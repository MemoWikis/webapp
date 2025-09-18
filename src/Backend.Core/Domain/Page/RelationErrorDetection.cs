public class RelationErrorDetection : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Detects all relation errors across the system
    /// </summary>
    public RelationErrorsResult GetErrors()
    {
        Log.Information("Get Relation Errors - Start");
        var relationErrors = new List<RelationErrorItem>();

        try
        {
            var allRelations = EntityCache.GetAllRelations();
            var groupedByParent = allRelations.GroupBy(r => r.ParentId);

            foreach (var parentGroup in groupedByParent)
            {
                var parentId = parentGroup.Key;
                Log.Information("Get Relation Errors - Check page:{id}", parentId);

                var parentPage = EntityCache.GetPage(parentId);

                if (parentPage == null)
                    continue;

                var relations = parentGroup.ToList();
                var errors = DetectErrorsForParent(relations);

                // Convert relations to relationItems
                var relationItems = relations.Select(relation => new RelationItem(
                    relation.Id,
                    relation.PreviousId,
                    relation.NextId,
                    relation.ChildId,
                    relation.ParentId
                )).ToList();

                if (errors.Any())
                {
                    relationErrors.Add(new RelationErrorItem(
                        parentId,
                        errors,
                        relationItems
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

    private List<RelationError> DetectErrorsForParent(List<PageRelationCache> relations)
    {
        var errors = new List<RelationError>();

        if (!relations.Any())
            return errors;

        // Check for duplicate relations
        var duplicateGroups = relations
            .GroupBy(r => r.ChildId)
            .Where(g => g.Count() > 1);

        DetectDuplicates(duplicateGroups, errors);

        // Check for broken links (child pages that don't exist)
        DetectMissingChildren(relations, errors);

        // Check for ordering errors
        DetectOrderingErrors(relations, errors);

        return errors;
    }

    private static void DetectMissingChildren(List<PageRelationCache> relations, List<RelationError> errors)
    {
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
    }

    private static void DetectDuplicates(IEnumerable<IGrouping<int, PageRelationCache>> duplicateGroups, List<RelationError> errors)
    {
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
    }

    /// <summary>
    /// Detects various ordering errors in relations
    /// </summary>
    private void DetectOrderingErrors(List<PageRelationCache> relations, List<RelationError> errors)
    {
        // Check for multiple relations with null PreviousId (multiple "first" items)
        DetectMultipleFirstItems(relations, errors);

        // Check for multiple relations with null NextId (multiple "last" items)
        DetectMultipleLastItems(relations, errors);

        // Check for multiple relations with the same PreviousId
        DetectPreviousIdDuplicates(relations, errors);

        // Check for multiple relations with the same NextId
        DetectNextIdDuplicates(relations, errors);

        // Check for inconsistent chain links
        DetectInconsistentLinks(relations, errors);

        // Check for circular references
        DetectCircularReferences(relations, errors);

        // Check for orphaned relations (not connected to the main chain)
        DetectOrphanedRelations(relations, errors);
    }

    private static void DetectInconsistentLinks(List<PageRelationCache> relations, List<RelationError> errors)
    {
        foreach (var relation in relations)
        {
            // If this relation has a NextId, check if the target relation has this relation as PreviousId
            ValidateNextIdConsistency(relations, errors, relation);

            // If this relation has a PreviousId, check if the target relation has this relation as NextId
            ValidatePreviousIdConsistency(relations, errors, relation);
        }
    }

    private static void ValidatePreviousIdConsistency(List<PageRelationCache> relations, List<RelationError> errors,
        PageRelationCache relation)
    {
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

    private static void ValidateNextIdConsistency(List<PageRelationCache> relations, List<RelationError> errors, PageRelationCache relation)
    {
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
    }

    private static void DetectNextIdDuplicates(List<PageRelationCache> relations, List<RelationError> errors)
    {
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
    }

    private static void DetectPreviousIdDuplicates(List<PageRelationCache> relations, List<RelationError> errors)
    {
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
    }

    private static void DetectMultipleLastItems(List<PageRelationCache> relations, List<RelationError> errors)
    {
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
    }

    private static void DetectMultipleFirstItems(List<PageRelationCache> relations, List<RelationError> errors)
    {
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
            // Check if we've already visited this relation (cycle detection)
            if (connectedRelations.Contains(currentRelation.ChildId))
            {
                // We've found a cycle, stop traversing this chain
                break;
            }

            connectedRelations.Add(currentRelation.ChildId);

            if (currentRelation.NextId.HasValue)
            {
                currentRelation = relations.FirstOrDefault(r => r.ChildId == currentRelation.NextId.Value);
            }
            else
            {
                break;
            }
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
}
