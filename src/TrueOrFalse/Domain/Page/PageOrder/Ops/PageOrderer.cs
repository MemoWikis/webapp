public class PageOrderer
{
    public static bool CanBeMoved(int childId, int parentId)
    {
        if (childId == parentId)
            return false;

        if (GraphService.Descendants(childId).Any(r => r.Id == parentId))
            return false;

        return true;
    }

    public static void MoveIn(
        PageRelationCache relation,
        int newParentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage,
        PermissionCheck permissionCheck)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            Logg.r.Error(
                "PageRelations - moveIn: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Page.CircularReference);
        }

        modifyRelationsForPage.AddChild(newParentId, relation.ChildId, authorId);
        ModifyRelationsEntityCache.RemoveParent(
            EntityCache.GetPage(relation.ChildId),
            relation.ParentId,
            authorId,
            modifyRelationsForPage,
            permissionCheck);
    }

    public static (List<PageRelationCache> UpdatedOldOrder, List<PageRelationCache>
        UpdatedNewOrder) MoveBefore(
            PageRelationCache relation,
            int beforePageId,
            int newParentId,
            int authorId,
            ModifyRelationsForPage modifyRelationsForPage)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            Logg.r.Error(
                "PageRelations - MoveBefore: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Page.CircularReference);
        }

        var updatedNewOrder = AddBefore(relation.ChildId, beforePageId, newParentId, authorId,
            modifyRelationsForPage);
        var updatedOldOrder =
            Remove(relation, relation.ParentId, authorId, modifyRelationsForPage);

        return (updatedOldOrder, updatedNewOrder);
    }

    public static (List<PageRelationCache> UpdatedOldOrder, List<PageRelationCache>
        UpdatedNewOrder) MoveAfter(
            PageRelationCache relation,
            int afterPageId,
            int newParentId,
            int authorId,
            ModifyRelationsForPage modifyRelationsForPage)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            Logg.r.Error(
                "PageRelations - MoveAfter: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Page.CircularReference);
        }

        var updatedNewOrder = AddAfter(relation.ChildId, afterPageId, newParentId, authorId,
            modifyRelationsForPage);
        var updatedOldOrder =
            Remove(relation, relation.ParentId, authorId, modifyRelationsForPage);

        return (updatedOldOrder, updatedNewOrder);
    }

    public static List<PageRelationCache> Remove(
        PageRelationCache relation,
        int oldParentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var parent = EntityCache.GetPage(oldParentId);
        var relations = parent.ChildRelations;

        var relationIndex = relations.IndexOf(relation);
        if (relationIndex != -1)
        {
            var changedRelations = new List<PageRelationCache>();

            if (relationIndex > 0)
            {
                var previousRelation = relations[relationIndex - 1];
                previousRelation.NextId = relationIndex < relations.Count - 1
                    ? relations[relationIndex + 1].ChildId
                    : null;

                EntityCache.AddOrUpdate(previousRelation);
                changedRelations.Add(previousRelation);
            }

            if (relationIndex < relations.Count - 1)
            {
                var nextRelation = relations[relationIndex + 1];
                nextRelation.PreviousId = relationIndex > 0
                    ? relations[relationIndex - 1].ChildId
                    : null;

                EntityCache.AddOrUpdate(nextRelation);
                changedRelations.Add(nextRelation);
            }

            relations.RemoveAt(relationIndex);
            RemoveRelationFromParentRelations(relation);
            EntityCache.Remove(relation);
            parent.ChildRelations = relations;
            EntityCache.AddOrUpdate(parent);
            modifyRelationsForPage.UpdateRelationsInDb(changedRelations, authorId);
            modifyRelationsForPage.DeleteRelationInDb(relation.Id, authorId);
        }

        return relations.ToList();
    }

    private static void RemoveRelationFromParentRelations(PageRelationCache relation)
    {
        var child = EntityCache.GetPage(relation.ChildId);
        var relations = child.ParentRelations;

        var relationIndex = relations.IndexOf(relation);
        if (relationIndex >= 0)
            relations.RemoveAt(relationIndex);
    }

    private static List<PageRelationCache> AddBefore(
        int pageId,
        int beforePageId,
        int parentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var parent = EntityCache.GetPage(parentId);

        var relations = parent.ChildRelations.ToList();
        var newRelations =
            Insert(
                pageId,
                beforePageId,
                parentId,
                relations,
                false,
                authorId,
                modifyRelationsForPage);

        parent.ChildRelations = newRelations;
        return newRelations;
    }

    private static List<PageRelationCache> AddAfter(
        int pageId,
        int afterPageId,
        int parentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var parent = EntityCache.GetPage(parentId);

        var relations = parent.ChildRelations.ToList();
        var newRelations =
            Insert(
                pageId,
                afterPageId,
                parentId,
                relations,
                true,
                authorId,
                modifyRelationsForPage);

        parent.ChildRelations = newRelations;
        return newRelations;
    }

    private static List<PageRelationCache> Insert(
        int childId,
        int targetPageId,
        int parentId,
        List<PageRelationCache> relations,
        bool insertAfter,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var relation = new PageRelationCache
        {
            ChildId = childId,
            ParentId = parentId,
            PreviousId = insertAfter ? targetPageId : null,
            NextId = !insertAfter ? targetPageId : null,
        };

        var targetPosition = relations.FindIndex(r => r.ChildId == targetPageId);
        if (targetPosition == -1)
        {
            Logg.r.Error(
                "PageRelations - Insert: Targetposition not found - parentId:{0}, targetPageId:{1}",
                parentId, targetPageId);
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);
        }

        var positionToInsert = insertAfter ? targetPosition + 1 : targetPosition;

        relations.Insert(positionToInsert, relation);

        var currentRelation = relations[positionToInsert];
        var changedRelations = new List<PageRelationCache>();

        if (insertAfter)
            InsertAfter(
                relations,
                targetPosition,
                currentRelation,
                changedRelations,
                positionToInsert);
        else
            InsertBefore(
                relations,
                positionToInsert,
                currentRelation,
                changedRelations);

        var newRelationId = modifyRelationsForPage.CreateNewRelationAndGetId(
            currentRelation.ParentId, currentRelation.ChildId,
            currentRelation.NextId, currentRelation.PreviousId);

        currentRelation.Id = newRelationId;

        EntityCache.AddOrUpdate(currentRelation);
        EntityCache.GetPage(currentRelation.ChildId)?.ParentRelations.Add(currentRelation);
        modifyRelationsForPage.UpdateRelationsInDb(changedRelations, authorId);

        return relations;
    }

    private static void InsertBefore(
        List<PageRelationCache> relations,
        int positionToInsert,
        PageRelationCache current,
        List<PageRelationCache> changedRelations)
    {
        var nextRelation = relations[positionToInsert + 1];
        nextRelation.PreviousId = current.ChildId;

        EntityCache.AddOrUpdate(nextRelation);
        changedRelations.Add(nextRelation);

        if (positionToInsert > 0) // updates the relation before the newly inserted relation
        {
            var previousRelation = relations[positionToInsert - 1];
            previousRelation.NextId = current.ChildId;
            current.PreviousId = previousRelation.ChildId;

            EntityCache.AddOrUpdate(previousRelation);
            changedRelations.Add(previousRelation);
        }
    }

    private static void InsertAfter(
        List<PageRelationCache> relations,
        int targetPosition,
        PageRelationCache current,
        List<PageRelationCache> changedRelations,
        int positionToInsert)
    {
        var previousRelation = relations[targetPosition];
        previousRelation.NextId = current.ChildId;

        EntityCache.AddOrUpdate(previousRelation);
        changedRelations.Add(previousRelation);

        if (positionToInsert + 1 <
            relations.Count) // updates the relation after the newly inserted relation
        {
            var nextRelation = relations[positionToInsert + 1];
            nextRelation.PreviousId = current.ChildId;
            current.NextId = nextRelation.ChildId;

            EntityCache.AddOrUpdate(nextRelation);
            changedRelations.Add(nextRelation);
        }
    }

    public static IList<PageRelationCache> Sort(int pageId)
    {
        var childRelations = EntityCache.GetChildRelationsByParentId(pageId);
        return childRelations.Count <= 0 ? childRelations : Sort(childRelations, pageId);
    }

    public static IList<PageRelationCache> Sort(
        IList<PageRelationCache> childRelations,
        int pageId)
    {
        var currentRelation = childRelations.FirstOrDefault(r => r.PreviousId == null);
        var sortedRelations = new List<PageRelationCache>();
        var addedRelationIds = new HashSet<int>();
        var addedChildIds = new HashSet<int>();

        while (currentRelation != null)
        {
            var nextCurrentRelation = childRelations.FirstOrDefault(r =>
                r.ChildId == currentRelation.NextId);

            if (addedChildIds.Contains(currentRelation.ChildId))
            {
                addedRelationIds.Add(currentRelation.Id);

                Logg.r.Error(
                    "PageRelations - Sort: Force break 'while loop', duplicate child - PageId:{0}, RelationId: {1}",
                    pageId, currentRelation.Id);

                break;
            }

            addedRelationIds.Add(currentRelation.Id);
            addedChildIds.Add(currentRelation.ChildId);

            sortedRelations.Add(currentRelation);

            currentRelation = nextCurrentRelation;


            if (addedRelationIds.Count >= childRelations.Count && currentRelation != null)
            {
                Logg.r.Error(
                    "PageRelations - Sort: Force break 'while loop', faulty links - PageId:{0}, RelationId: {1}",
                    pageId, currentRelation.Id);
                break;
            }
        }

        if (sortedRelations.Count < childRelations.Count)
            AppendMissingRelations(pageId, sortedRelations, childRelations, addedRelationIds, addedChildIds);

        return sortedRelations;
    }

    private static void AppendMissingRelations(
        int pageId,
        List<PageRelationCache> sortedRelations,
        IList<PageRelationCache> childRelations,
        HashSet<int> addedRelationIds,
        HashSet<int> addedChildIds)
    {
        Logg.r.Error("PageRelations - Sort: Broken Link Start - PageId:{0}, RelationId:{1}",
            pageId, sortedRelations.LastOrDefault()?.Id);

        foreach (var childRelation in childRelations)
        {
            if (!addedRelationIds.Contains(childRelation.Id))
            {
                if (!addedChildIds.Contains(childRelation.ChildId))
                    sortedRelations.Add(childRelation);

                Logg.r.Error("PageRelations - Sort: Broken Link - PageId:{0}, RelationId:{1}",
                    pageId, childRelation.Id);
            }
        }

        Logg.r.Error("PageRelations - Sort: Broken Link End - PageId:{0}", pageId);
    }

    public static List<PageRelationCache> RepairRelationOrder(
        int parentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var parent = EntityCache.GetPage(parentId);
        if (parent == null)
        {
            Logg.r.Error("PageRelations - RepairRelationOrder: Parent not found - parentId:{0}", parentId);
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);
        }

        var childRelations = parent.ChildRelations.ToList();
        var dbRelations = modifyRelationsForPage.GetRelationsByParentId(parentId);

        Logg.r.Information("PageRelations - RepairRelationOrder: Starting repair for parentId:{0} with {1} cache relations and {2} db relations",
            parentId, childRelations.Count, dbRelations.Count);

        if (childRelations.Count == 0 && dbRelations.Count == 0)
            return childRelations;

        var deduplicatedRelations = RemoveDuplicateRelations(childRelations, parentId, authorId, modifyRelationsForPage);
        var repairedRelations = RepairBrokenLinks(deduplicatedRelations, parentId);
        var syncedRelations = SyncDatabaseWithCache(repairedRelations, dbRelations, parentId, authorId, modifyRelationsForPage);

        parent.ChildRelations = syncedRelations;
        EntityCache.AddOrUpdate(parent);

        return syncedRelations;
    }

    private static List<PageRelationCache> RemoveDuplicateRelations(
        List<PageRelationCache> childRelations,
        int parentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var usedChildIds = new HashSet<int>();
        var relationsToDelete = new List<PageRelationCache>();
        var uniqueRelations = new List<PageRelationCache>();

        foreach (var relation in childRelations)
        {
            if (usedChildIds.Contains(relation.ChildId))
            {
                Logg.r.Warning("PageRelations - RemoveDuplicateRelations: Found duplicate relation - RelationId:{0}, ChildId:{1}",
                    relation.Id, relation.ChildId);
                relationsToDelete.Add(relation);
            }
            else
            {
                usedChildIds.Add(relation.ChildId);
                uniqueRelations.Add(relation);
            }
        }

        foreach (var relationToDelete in relationsToDelete)
        {
            RemoveRelationFromParentRelations(relationToDelete);
            EntityCache.Remove(relationToDelete);

            modifyRelationsForPage.DeleteRelationInDb(relationToDelete.Id, authorId);
            Logg.r.Information("PageRelations - RemoveDuplicateRelations: Deleted duplicate relation - RelationId:{0}, ChildId:{1}",
                relationToDelete.Id, relationToDelete.ChildId);
        }

        return uniqueRelations;
    }

    private static List<PageRelationCache> RepairBrokenLinks(
        List<PageRelationCache> relations,
        int parentId)
    {
        var changedRelations = new List<PageRelationCache>();
        var processedRelationIds = new HashSet<int>();
        var orderedRelations = new List<PageRelationCache>();

        var currentRelation = FindStartingRelation(relations);
        PageRelationCache? previousRelation = null;

        while (currentRelation != null && !processedRelationIds.Contains(currentRelation.Id))
        {
            processedRelationIds.Add(currentRelation.Id);
            orderedRelations.Add(currentRelation);

            if (previousRelation != null && currentRelation.PreviousId != previousRelation.ChildId)
            {
                currentRelation.PreviousId = previousRelation.ChildId;
                changedRelations.Add(currentRelation);
            }

            previousRelation = currentRelation;
            currentRelation = FindNextRelation(relations, currentRelation);
        }

        HandleOrphanedRelations(
            relations,
            orderedRelations,
            processedRelationIds,
            changedRelations,
            parentId);

        if (orderedRelations.Any() && orderedRelations.First().PreviousId != null)
        {
            orderedRelations.First().PreviousId = null;
            changedRelations.Add(orderedRelations.First());
        }

        if (orderedRelations.Any() && orderedRelations.Last().NextId != null)
        {
            orderedRelations.Last().NextId = null;
            changedRelations.Add(orderedRelations.Last());
        }

        foreach (var relation in changedRelations.Distinct())
        {
            EntityCache.AddOrUpdate(relation);
        }

        return orderedRelations;
    }

    private static void HandleOrphanedRelations(
        List<PageRelationCache> allRelations,
        List<PageRelationCache> orderedRelations,
        HashSet<int> processedRelationIds,
        List<PageRelationCache> changedRelations,
        int parentId)
    {
        var orphanedRelations = allRelations
            .Where(r => !processedRelationIds.Contains(r.Id))
            .OrderBy(r => r.ChildId)
            .ToList();

        if (!orphanedRelations.Any())
            return;

        Logg.r.Warning("PageRelations - HandleOrphanedRelations: Found {0} orphaned relations for parentId:{1}",
            orphanedRelations.Count, parentId);

        if (orderedRelations.Any())
        {
            AppendOrphansToExistingChain(orphanedRelations, orderedRelations, changedRelations);
        }
        else
        {
            BuildChainFromOrphans(orphanedRelations, orderedRelations, changedRelations);
        }
    }

    private static void AppendOrphansToExistingChain(
        List<PageRelationCache> orphanedRelations,
        List<PageRelationCache> orderedRelations,
        List<PageRelationCache> changedRelations)
    {
        var lastRelation = orderedRelations.Last();

        var firstOrphan = orphanedRelations.First();
        firstOrphan.PreviousId = lastRelation.ChildId;
        lastRelation.NextId = firstOrphan.ChildId;

        changedRelations.Add(lastRelation);
        changedRelations.Add(firstOrphan);

        orderedRelations.Add(firstOrphan);
        var previousRelation = firstOrphan;

        foreach (var orphan in orphanedRelations.Skip(1))
        {
            orphan.PreviousId = previousRelation.ChildId;
            previousRelation.NextId = orphan.ChildId;

            changedRelations.Add(previousRelation);
            changedRelations.Add(orphan);

            orderedRelations.Add(orphan);
            previousRelation = orphan;
        }
    }

    private static void BuildChainFromOrphans(
        List<PageRelationCache> orphanedRelations,
        List<PageRelationCache> orderedRelations,
        List<PageRelationCache> changedRelations)
    {
        PageRelationCache? prev = null;

        foreach (var orphan in orphanedRelations)
        {
            orphan.PreviousId = prev?.ChildId;
            if (prev != null)
            {
                prev.NextId = orphan.ChildId;
                changedRelations.Add(prev);
            }

            changedRelations.Add(orphan);
            orderedRelations.Add(orphan);
            prev = orphan;
        }
    }

    private static PageRelationCache? FindStartingRelation(List<PageRelationCache> relations)
    {
        return relations.FirstOrDefault(r => r.PreviousId == null);
    }

    private static PageRelationCache? FindNextRelation(List<PageRelationCache> relations, PageRelationCache currentRelation)
    {
        return relations.FirstOrDefault(r => r.ChildId == currentRelation.NextId);
    }

    private static List<PageRelationCache> SyncDatabaseWithCache(
        List<PageRelationCache> cacheRelations,
        List<PageRelationCache> dbRelations,
        int parentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var changedRelations = new List<PageRelationCache>();

        DeleteOrphanedDatabaseRelations(cacheRelations, dbRelations, authorId, modifyRelationsForPage);
        CreateMissingDatabaseRelations(cacheRelations, dbRelations, authorId, modifyRelationsForPage);
        modifyRelationsForPage.UpdateRelationsInDb(changedRelations.Distinct().ToList(), authorId);

        return cacheRelations;
    }

    private static void DeleteOrphanedDatabaseRelations(
        List<PageRelationCache> cacheRelations,
        List<PageRelationCache> dbRelations,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var cacheRelationIds = cacheRelations.Select(r => r.Id).ToHashSet();
        var orphanedDbRelations = dbRelations.Where(r => !cacheRelationIds.Contains(r.Id)).ToList();

        foreach (var orphanedDbRelation in orphanedDbRelations)
        {
            modifyRelationsForPage.DeleteRelationInDb(orphanedDbRelation.Id, authorId);
            Logg.r.Warning("PageRelations - DeleteOrphanedDatabaseRelations: Deleted orphaned database relation - RelationId:{0}, ChildId:{1}",
                orphanedDbRelation.Id, orphanedDbRelation.ChildId);
        }
    }

    private static void CreateMissingDatabaseRelations(
        List<PageRelationCache> cacheRelations,
        List<PageRelationCache> dbRelations,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var dbRelationIds = dbRelations.Select(r => r.Id).ToHashSet();
        var missingDbRelations = cacheRelations.Where(r => !dbRelationIds.Contains(r.Id)).ToList();

        foreach (var missingDbRelation in missingDbRelations)
        {
            var newRelationId = modifyRelationsForPage.CreateNewRelationAndGetId(
                missingDbRelation.ParentId,
                missingDbRelation.ChildId,
                missingDbRelation.NextId,
                missingDbRelation.PreviousId);

            missingDbRelation.Id = newRelationId;
            EntityCache.AddOrUpdate(missingDbRelation);

            Logg.r.Warning("PageRelations - CreateMissingDatabaseRelations: Created missing database relation - RelationId:{0}, ChildId:{1}",
                newRelationId, missingDbRelation.ChildId);
        }
    }


}