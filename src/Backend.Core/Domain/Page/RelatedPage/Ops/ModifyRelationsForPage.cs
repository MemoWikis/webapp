public class ModifyRelationsForPage(
    PageRepository pageRepository,
    PageRelationRepo pageRelationRepo,
    KnowledgeSummaryUpdateService? knowledgeSummaryUpdateService = null) : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Updates relations with related pages (keeps existing and deletes missing) with possible restrictions on type of relation (IsChildOf etc.) and type of page (Standard, Book etc.)
    /// </summary>
    /// <param name="pageCacheItem"></param>
    /// <param name="relatedPageIds">Existing relations are updated with this collection (existing are kept, non-included are deleted)</param>
    /// <param name="relationType">If specified only relations of this type will be updated</param>
    public void AddParentPage(Page page, int parentId)
    {
        var relatedPage = pageRepository.GetByIdEager(parentId);
        var previousCachedRelation = EntityCache.GetPage(parentId)?.ChildRelations.LastOrDefault();

        if (previousCachedRelation != null)
        {
            var previousRelation = pageRelationRepo.GetById(previousCachedRelation.Id);
            if (previousRelation != null)
            {
                previousRelation.NextId = page.Id;

                EntityCache.AddOrUpdate(previousCachedRelation);
                pageRelationRepo.Update(previousRelation);
            }
        }

        var pageRelationToAdd = new PageRelation()
        {
            Child = page,
            Parent = relatedPage,
            PreviousId = previousCachedRelation?.ChildId
        };

        pageRelationRepo.Create(pageRelationToAdd);

        // Schedule knowledge summary update only for parent (parent gains new content from child)
        knowledgeSummaryUpdateService?.SchedulePageUpdate(parentId);
    }

    public void AddChild(int parentId, int childId, int authorId)
    {
        var cachedParent = EntityCache.GetPage(parentId);
        var previousCacheRelation = cachedParent?.ChildRelations.LastOrDefault();

        var child = pageRepository.GetById(childId);
        var parent = pageRepository.GetById(parentId);

        var relation = new PageRelation
        {
            Child = child,
            Parent = parent,
            PreviousId = previousCacheRelation?.ChildId,
            NextId = null,
        };

        pageRelationRepo.Create(relation);

        if (previousCacheRelation != null)
            UpdatePreviousCacheRelationOnAddChild(childId, previousCacheRelation);

        ModifyRelationsEntityCache.AddChild(relation);
        pageRepository.UpdateChildAndParentForRelations(child, parent, authorId);

        // Schedule knowledge summary update only for parent (parent gains new content from child)
        knowledgeSummaryUpdateService?.SchedulePageUpdate(parentId);
    }

    private void UpdatePreviousCacheRelationOnAddChild(int childId, PageRelationCache previousCacheRelation)
    {
        var previousRelation = pageRelationRepo.GetById(previousCacheRelation.Id);
        previousCacheRelation.NextId = childId;
        EntityCache.AddOrUpdate(previousCacheRelation);

        if (previousRelation != null)
        {
            previousRelation.NextId = childId;
            pageRelationRepo.Update(previousRelation);
        }
    }

    public int CreateNewRelationAndGetId(int parentId, int childId, int? nextId, int? previousId)
    {
        var child = pageRepository.GetById(childId);
        var parent = pageRepository.GetById(parentId);

        var relation = new PageRelation
        {
            Child = child,
            Parent = parent,
            PreviousId = previousId,
            NextId = nextId,
        };

        pageRelationRepo.Create(relation);
        return relation.Id;
    }

    public void UpdateRelationsInDb(List<PageRelationCache> cachedRelations, int authorId)
    {
        var parentPageIds = new HashSet<int>();

        foreach (var r in cachedRelations)
        {
            Log.Information(
                "ModifyRelations RelationId: {relationId}, Child: {childId}, Parent: {parentId}",
                r.Id, r.ChildId, r.ParentId);

            var relationToUpdate = pageRelationRepo.GetById(r.Id);

            if (relationToUpdate != null)
            {
                var child = pageRepository.GetById(r.ChildId);
                var parent = pageRepository.GetById(r.ParentId);

                relationToUpdate.Child = child;
                relationToUpdate.Parent = parent;
                relationToUpdate.PreviousId = r.PreviousId;
                relationToUpdate.NextId = r.NextId;

                pageRelationRepo.Update(relationToUpdate);

                pageRepository.Update(child, authorId, type: PageChangeType.Relations);
                pageRepository.Update(parent, authorId, type: PageChangeType.Relations);

                // Only collect parent page IDs for knowledge summary updates
                parentPageIds.Add(r.ParentId);
            }
        }

        // Schedule knowledge summary updates only for parent pages (they gain/lose child content)
        foreach (var parentPageId in parentPageIds)
        {
            knowledgeSummaryUpdateService?.SchedulePageUpdate(parentPageId);
        }
    }

    public void DeleteRelationInDb(int relationId, int authorId)
    {
        var relationToDelete = relationId > 0 ? pageRelationRepo.GetById(relationId) : null;
        Log.Information("DeleteRelation RelationId: {relationId}, Child: {childId}, Parent: {parentId}", relationToDelete?.Id, relationToDelete?.Child?.Id, relationToDelete?.Parent?.Id);

        if (relationToDelete != null)
        {
            var parentId = relationToDelete.Parent?.Id;

            pageRelationRepo.Delete(relationToDelete);
            pageRepository.Update(relationToDelete.Child, authorId, type: PageChangeType.Relations);
            pageRepository.Update(relationToDelete.Parent, authorId, type: PageChangeType.Relations);

            // Schedule knowledge summary update only for parent (parent loses child content)
            if (parentId.HasValue)
                knowledgeSummaryUpdateService?.SchedulePageUpdate(parentId.Value);
        }
    }
}