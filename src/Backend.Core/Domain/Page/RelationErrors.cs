public class RelationErrors(PageRelationRepo _pageRelationRepo)
{
    public readonly record struct RelationErrorsResult(bool Success, List<RelationErrorItem> Data);

    public readonly record struct RelationErrorItem(
        int ParentId,
        List<RelationError> Errors,
        List<RelationTableItem> Relations);

    public readonly record struct RelationError(string Type, int ChildId, string Description);

    public readonly record struct RelationTableItem(
        int RelationId,
        int? PreviousId,
        int? NextId,
        int ChildId,
        int ParentId);

    public readonly record struct HealResult(bool Success, string Message, int HealedCount);

    /// <summary>
    /// Detects all relation errors across the system
    /// </summary>
    public RelationErrorsResult GetErrors()
    {
        var relationErrors = new List<RelationErrorItem>();

        try
        {
            var allRelations = EntityCache.GetAllRelations();
            var groupedByParent = allRelations.GroupBy(r => r.ParentId);

            foreach (var parentGroup in groupedByParent)
            {
                var parentId = parentGroup.Key;
                var parentPage = EntityCache.GetPage(parentId);

                if (parentPage == null)
                    continue;

                var relations = parentGroup.ToList();
                var errors = DetectErrorsForParent(relations);

                // Convert relations to table items
                var relationTableItems = relations.Select(r => new RelationTableItem(
                    r.Id,
                    r.PreviousId,
                    r.NextId,
                    r.ChildId,
                    r.ParentId
                )).ToList();

                if (errors.Any())
                {
                    relationErrors.Add(new RelationErrorItem(
                        parentId,
                        errors,
                        relationTableItems
                    ));
                }
            }

            return new RelationErrorsResult(true, relationErrors);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while checking relation errors");
            return new RelationErrorsResult(false, new List<RelationErrorItem>());
        }
    }

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
                return new HealResult(false, "Page not found.", 0);
            }

            var relations = EntityCache.GetCacheRelationsByParentId(parentPageId);
            var healedCount = 0;

            // Remove duplicate relations
            healedCount += RemoveDuplicateRelations(relations);

            // Refresh relations after duplicates removal
            relations = EntityCache.GetCacheRelationsByParentId(parentPageId);

            // Remove relations with broken links (non-existent child pages)
            healedCount += RemoveBrokenLinkRelations(relations);

            // Refresh relations after broken links removal
            relations = EntityCache.GetCacheRelationsByParentId(parentPageId);

            // Fix ordering issues for this specific parent only
            healedCount += HealOrderingErrorsForParent(parentPageId);

            return new HealResult(
                true,
                $"Relations repaired. {healedCount} issues fixed.",
                healedCount
            );
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while healing relations for page {PageId}", parentPageId);
            return new HealResult(false, "Error while repairing relations.", 0);
        }
    }

    /// <summary>
    /// Detects errors for relations under a specific parent
    /// </summary>
    private List<RelationError> DetectErrorsForParent(List<PageRelationCache> relations)
    {
        var errors = new List<RelationError>();

        if (!relations.Any())
            return errors;

        // Check for duplicate relations
        var duplicateGroups = relations
            .GroupBy(r => r.ChildId)
            .Where(g => g.Count() > 1);

        foreach (var duplicateGroup in duplicateGroups)
        {
            var childId = duplicateGroup.Key;
            var count = duplicateGroup.Count();

            errors.Add(new RelationError(
                "Duplicate",
                childId,
                $"Child page (ID: {childId}) appears {count} times"
            ));
        }

        // Check for broken links (child pages that don't exist)
        foreach (var relation in relations)
        {
            var childPage = EntityCache.GetPage(relation.ChildId);
            if (childPage == null)
            {
                errors.Add(new RelationError(
                    "BrokenLink",
                    relation.ChildId,
                    "Child page does not exist"
                ));
            }
        }

        // Check for ordering errors
        DetectOrderingErrors(relations, errors);

        return errors;
    }

    /// <summary>
    /// Detects various ordering errors in relations
    /// </summary>
    private void DetectOrderingErrors(List<PageRelationCache> relations, List<RelationError> errors)
    {
        // Check for multiple relations with null PreviousId (multiple "first" items)
        var relationsWithNullPrevious = relations.Where(r => r.PreviousId == null).ToList();
        if (relationsWithNullPrevious.Count > 1)
        {
            foreach (var relation in relationsWithNullPrevious)
            {
                errors.Add(new RelationError(
                    "MultipleFirstItems",
                    relation.ChildId,
                    $"Multiple relations have null PreviousId (child: {relation.ChildId})"
                ));
            }
        }

        // Check for multiple relations with null NextId (multiple "last" items)
        var relationsWithNullNext = relations.Where(r => r.NextId == null).ToList();
        if (relationsWithNullNext.Count > 1)
        {
            foreach (var relation in relationsWithNullNext)
            {
                errors.Add(new RelationError(
                    "MultipleLastItems",
                    relation.ChildId,
                    $"Multiple relations have null NextId (child: {relation.ChildId})"
                ));
            }
        }

        // Check for multiple relations with the same PreviousId
        var previousIdGroups = relations
            .Where(r => r.PreviousId.HasValue)
            .GroupBy(r => r.PreviousId!.Value)
            .Where(g => g.Count() > 1);

        foreach (var previousIdGroup in previousIdGroups)
        {
            var previousId = previousIdGroup.Key;
            foreach (var relation in previousIdGroup)
            {
                errors.Add(new RelationError(
                    "DuplicatePreviousId",
                    relation.ChildId,
                    $"Multiple relations have the same PreviousId: {previousId} (child: {relation.ChildId})"
                ));
            }
        }

        // Check for multiple relations with the same NextId
        var nextIdGroups = relations
            .Where(r => r.NextId.HasValue)
            .GroupBy(r => r.NextId!.Value)
            .Where(g => g.Count() > 1);

        foreach (var nextIdGroup in nextIdGroups)
        {
            var nextId = nextIdGroup.Key;
            foreach (var relation in nextIdGroup)
            {
                errors.Add(new RelationError(
                    "DuplicateNextId",
                    relation.ChildId,
                    $"Multiple relations have the same NextId: {nextId} (child: {relation.ChildId})"
                ));
            }
        }

        // Check for inconsistent chain links
        foreach (var relation in relations)
        {
            // If this relation has a NextId, check if the target relation has this relation as PreviousId
            if (relation.NextId.HasValue)
            {
                var nextRelation = relations.FirstOrDefault(r => r.ChildId == relation.NextId.Value);
                if (nextRelation != null && nextRelation.PreviousId != relation.ChildId)
                {
                    errors.Add(new RelationError(
                        "InconsistentChainLink",
                        relation.ChildId,
                        $"Relation points to NextId {relation.NextId}, but target doesn't point back (child: {relation.ChildId})"
                    ));
                }
                else if (nextRelation == null)
                {
                    errors.Add(new RelationError(
                        "BrokenNextLink",
                        relation.ChildId,
                        $"Relation points to non-existent NextId: {relation.NextId} (child: {relation.ChildId})"
                    ));
                }
            }

            // If this relation has a PreviousId, check if the target relation has this relation as NextId
            if (relation.PreviousId.HasValue)
            {
                var previousRelation = relations.FirstOrDefault(r => r.ChildId == relation.PreviousId.Value);
                if (previousRelation != null && previousRelation.NextId != relation.ChildId)
                {
                    errors.Add(new RelationError(
                        "InconsistentChainLink",
                        relation.ChildId,
                        $"Relation points to PreviousId {relation.PreviousId}, but target doesn't point back (child: {relation.ChildId})"
                    ));
                }
                else if (previousRelation == null)
                {
                    errors.Add(new RelationError(
                        "BrokenPreviousLink",
                        relation.ChildId,
                        $"Relation points to non-existent PreviousId: {relation.PreviousId} (child: {relation.ChildId})"
                    ));
                }
            }
        }

        // Check for circular references
        DetectCircularReferences(relations, errors);

        // Check for orphaned relations (not connected to the main chain)
        DetectOrphanedRelations(relations, errors);
    }

    /// <summary>
    /// Detects circular references in relation chains
    /// </summary>
    private void DetectCircularReferences(List<PageRelationCache> relations, List<RelationError> errors)
    {
        var visited = new HashSet<int>();
        var visiting = new HashSet<int>();

        foreach (var relation in relations)
        {
            if (!visited.Contains(relation.ChildId))
            {
                if (HasCircularReference(relation, relations, visited, visiting))
                {
                    errors.Add(new RelationError(
                        "CircularReference",
                        relation.ChildId,
                        $"Circular reference detected in chain involving child: {relation.ChildId}"
                    ));
                }
            }
        }
    }

    /// <summary>
    /// Checks if a relation is part of a circular reference using DFS
    /// </summary>
    private bool HasCircularReference(PageRelationCache startRelation, List<PageRelationCache> allRelations, 
        HashSet<int> visited, HashSet<int> visiting)
    {
        if (visiting.Contains(startRelation.ChildId))
            return true;

        if (visited.Contains(startRelation.ChildId))
            return false;

        visiting.Add(startRelation.ChildId);

        // Follow the NextId chain
        if (startRelation.NextId.HasValue)
        {
            var nextRelation = allRelations.FirstOrDefault(r => r.ChildId == startRelation.NextId.Value);
            if (nextRelation != null && HasCircularReference(nextRelation, allRelations, visited, visiting))
            {
                visiting.Remove(startRelation.ChildId);
                return true;
            }
        }

        visiting.Remove(startRelation.ChildId);
        visited.Add(startRelation.ChildId);
        return false;
    }

    /// <summary>
    /// Detects relations that are not connected to the main chain
    /// </summary>
    private void DetectOrphanedRelations(List<PageRelationCache> relations, List<RelationError> errors)
    {
        if (relations.Count <= 1)
            return;

        // Find the start of the chain (relation with null PreviousId)
        var chainStart = relations.FirstOrDefault(r => r.PreviousId == null);
        if (chainStart == null)
        {
            // No clear start, all relations might be orphaned or in a cycle
            errors.Add(new RelationError(
                "NoChainStart",
                0,
                "No relation found with null PreviousId - chain has no clear start"
            ));
            return;
        }

        // Follow the chain and mark connected relations
        var connectedRelations = new HashSet<int>();
        var currentRelation = chainStart;

        while (currentRelation != null)
        {
            connectedRelations.Add(currentRelation.ChildId);

            if (currentRelation.NextId.HasValue)
            {
                currentRelation = relations.FirstOrDefault(r => r.ChildId == currentRelation.NextId.Value);
            }
            else
            {
                break;
            }

            // Prevent infinite loops
            if (connectedRelations.Count > relations.Count)
                break;
        }

        // Find orphaned relations
        var orphanedRelations = relations.Where(r => !connectedRelations.Contains(r.ChildId));
        foreach (var orphanedRelation in orphanedRelations)
        {
            errors.Add(new RelationError(
                "OrphanedRelation",
                orphanedRelation.ChildId,
                $"Relation is not connected to the main chain (child: {orphanedRelation.ChildId})"
            ));
        }
    }

    /// <summary>
    /// Removes duplicate relations for the same parent-child pair
    /// </summary>
    private int RemoveDuplicateRelations(List<PageRelationCache> relations)
    {
        var healedCount = 0;
        var duplicateGroups = relations
            .GroupBy(r => r.ChildId)
            .Where(g => g.Count() > 1);

        foreach (var duplicateGroup in duplicateGroups)
        {
            var relationsToKeep = duplicateGroup.First();
            var relationsToDelete = duplicateGroup.Skip(1);

            foreach (var relationToDelete in relationsToDelete)
            {
                var dbRelation = _pageRelationRepo.GetById(relationToDelete.Id);
                if (dbRelation != null)
                {
                    _pageRelationRepo.Delete(dbRelation);
                    EntityCache.Remove(relationToDelete);
                    healedCount++;
                }
            }
        }

        return healedCount;
    }

    /// <summary>
    /// Removes relations that point to non-existent child pages
    /// </summary>
    private int RemoveBrokenLinkRelations(List<PageRelationCache> relations)
    {
        var healedCount = 0;
        var brokenRelations = relations.Where(r => EntityCache.GetPage(r.ChildId) == null);

        foreach (var brokenRelation in brokenRelations)
        {
            var dbRelation = _pageRelationRepo.GetById(brokenRelation.Id);
            if (dbRelation != null)
            {
                _pageRelationRepo.Delete(dbRelation);
                EntityCache.Remove(brokenRelation);
                healedCount++;
            }
        }

        return healedCount;
    }

    /// <summary>
    /// Heals ordering errors using a hybrid approach: preserve working chain segments, rebuild broken parts
    /// </summary>
    private int HealOrderingErrorsForParent(int parentPageId)
    {
        var relations = EntityCache.GetCacheRelationsByParentId(parentPageId);
        
        if (!relations.Any())
            return 0;

        var healedCount = 0;

        // Step 1: Build relation lookup for faster access
        var relationLookup = relations.ToDictionary(r => r.ChildId);

        // Step 2: Follow the valid chain starting from PreviousId = null
        var validChainOrder = new List<PageRelationCache>();
        var processedRelations = new HashSet<int>();
        
        // Find the start of the chain
        var chainStart = relations.FirstOrDefault(r => r.PreviousId == null);
        if (chainStart != null)
        {
            var currentRelation = chainStart;
            
            // Follow the chain as long as it's valid
            while (currentRelation != null)
            {
                validChainOrder.Add(currentRelation);
                processedRelations.Add(currentRelation.ChildId);
                
                // Check if next link is valid
                if (currentRelation.NextId.HasValue && 
                    relationLookup.TryGetValue(currentRelation.NextId.Value, out var nextRelation) &&
                    nextRelation.PreviousId == currentRelation.ChildId &&
                    !processedRelations.Contains(nextRelation.ChildId)) // Prevent circular references
                {
                    currentRelation = nextRelation;
                }
                else
                {
                    // Chain is broken, stop following
                    break;
                }
            }
        }

        // Step 3: Find remaining unprocessed relations (broken parts)
        var brokenRelations = relations.Where(r => !processedRelations.Contains(r.ChildId)).ToList();

        // Step 4: If there are broken relations, append them to the valid chain in a logical order
        if (brokenRelations.Any())
        {
            // Order broken relations by ChildId for consistency
            var orderedBrokenRelations = brokenRelations
                .GroupBy(r => r.ChildId)
                .Select(g => g.First()) // Remove any remaining duplicates
                .OrderBy(r => r.ChildId)
                .ToList();
            
            validChainOrder.AddRange(orderedBrokenRelations);
        }

        // Step 5: Now rebuild the chain links for the complete ordered list
        for (int i = 0; i < validChainOrder.Count; i++)
        {
            var relation = validChainOrder[i];
            var dbRelation = _pageRelationRepo.GetById(relation.Id);

            if (dbRelation != null)
            {
                var newPreviousId = i > 0 ? (int?)validChainOrder[i - 1].ChildId : null;
                var newNextId = i < validChainOrder.Count - 1 ? (int?)validChainOrder[i + 1].ChildId : null;

                // Only update if something actually changed
                if (relation.PreviousId != newPreviousId || relation.NextId != newNextId)
                {
                    relation.PreviousId = newPreviousId;
                    relation.NextId = newNextId;
                    dbRelation.PreviousId = newPreviousId;
                    dbRelation.NextId = newNextId;
                    _pageRelationRepo.Update(dbRelation);
                    EntityCache.AddOrUpdate(relation);
                    healedCount++;
                }
            }
        }

        return healedCount;
    }

    /// <summary>
    /// Fixes the ordering of relations by resetting PreviousId/NextId values
    /// [DEPRECATED - Use HealOrderingErrorsForParent instead]
    /// </summary>
    private void FixRelationOrdering(int parentPageId)
    {
        HealOrderingErrorsForParent(parentPageId);
    }
}