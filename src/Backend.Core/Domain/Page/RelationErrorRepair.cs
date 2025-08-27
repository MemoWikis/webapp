public class RelationErrorRepair(PageRelationRepo _pageRelationRepo, PageRepository _pageRepository) : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Heals relation errors for a specific parent page
    /// </summary>
    public HealResult HealErrors(int parentPageId)
    {
        try
        {
            var parentPage = EntityCache.GetPage(parentPageId);
            if (parentPage == null)
            {
                return new HealResult(false, "Page not found.");
            }

            // discard everything and rebuild from scratch
            HealOrderingErrorsForParent(parentPageId);

            return new HealResult(
                true,
                "Relations repaired successfully."
            );
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while healing relations for page {PageId}", parentPageId);
            return new HealResult(false, "Error while repairing relations.");
        }
    }

    /// <summary>
    /// Heals ordering errors using a nuclear approach: completely rebuild the relation chain from scratch
    /// </summary>
    private void HealOrderingErrorsForParent(int parentPageId)
    {
        var relations = EntityCache.GetCacheRelationsByParentId(parentPageId);

        if (!relations.Any())
            return;

        // Step 1: Gather all valid child IDs (no duplicates, only existing pages)
        var validChildIds = GatherValidChildIds(relations);

        // Step 2: Remove all existing relations for this parent
        RemoveAllExistingRelations(parentPageId);

        // Step 3: Create new properly chained relations
        CreateNewChainedRelations(parentPageId, validChildIds);
    }

    /// <summary>
    /// Step 1: Gathers valid child IDs, filtering out duplicates and broken links
    /// </summary>
    private List<int> GatherValidChildIds(List<PageRelationCache> relations)
    {
        var validChildIds = new List<int>();
        var processedChildIds = new HashSet<int>();

        foreach (var relation in relations)
        {
            // Skip if we've already processed this child ID
            if (processedChildIds.Contains(relation.ChildId))
            {
                continue; // Skip duplicate
            }

            // Check if the child page exists
            var childPage = EntityCache.GetPage(relation.ChildId);
            if (childPage == null)
            {
                continue; // Skip broken link
            }

            // Valid child - add to our new list
            validChildIds.Add(relation.ChildId);
            processedChildIds.Add(relation.ChildId);
        }

        return validChildIds;
    }

    /// <summary>
    /// Step 2: Removes all existing relations for the specified parent
    /// </summary>
    private void RemoveAllExistingRelations(int parentPageId)
    {
        // Get ALL database relations for this parent (not just cached ones)
        var allDbRelationsForParent = _pageRelationRepo.GetAll()
            .Where(r => r.Parent.Id == parentPageId)
            .ToList();

        foreach (var dbRelation in allDbRelationsForParent)
        {
            _pageRelationRepo.Delete(dbRelation);

            // Also remove from cache if it exists
            var cacheRelation = EntityCache.GetCacheRelationsByParentId(parentPageId)
                .FirstOrDefault(cr => cr.Id == dbRelation.Id);
            if (cacheRelation != null)
            {
                EntityCache.Remove(cacheRelation);
            }
        }
    }

    /// <summary>
    /// Step 3: Creates new properly chained relations for the valid child IDs
    /// </summary>
    private void CreateNewChainedRelations(int parentPageId, List<int> validChildIds)
    {
        for (int i = 0; i < validChildIds.Count; i++)
        {
            var childId = validChildIds[i];
            var previousId = i > 0 ? (int?)validChildIds[i - 1] : null;
            var nextId = i < validChildIds.Count - 1 ? (int?)validChildIds[i + 1] : null;

            // Get the actual page entities from repository
            var parentPage = _pageRepository.GetById(parentPageId);
            var childPage = _pageRepository.GetById(childId);

            if (parentPage == null || childPage == null)
                continue; // Skip if pages don't exist

            // Create new database relation
            var newDbRelation = new PageRelation
            {
                Parent = parentPage,
                Child = childPage,
                PreviousId = previousId,
                NextId = nextId
            };

            _pageRelationRepo.Create(newDbRelation);

            // Create new cache relation
            var newCacheRelation = new PageRelationCache
            {
                Id = newDbRelation.Id,
                ParentId = parentPageId,
                ChildId = childId,
                PreviousId = previousId,
                NextId = nextId
            };

            EntityCache.AddOrUpdate(newCacheRelation);
        }
    }
}
