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
            Log.Error(
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
            Log.Error(
                "PageRelations - MoveBefore: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Page.CircularReference);
        }

        var updatedNewOrder = AddBefore(
            relation.ChildId,
            beforePageId,
            newParentId,
            authorId,
            modifyRelationsForPage);

        var updatedOldOrder = Remove(
            relation,
            relation.ParentId,
            authorId,
            modifyRelationsForPage);

        return (updatedOldOrder, updatedNewOrder);
    }

    public static (List<PageRelationCache> UpdatedOldOrder, List<PageRelationCache> UpdatedNewOrder) MoveAfter(
        PageRelationCache relation,
        int afterPageId,
        int newParentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            Log.Error(
                "PageRelations - MoveAfter: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Page.CircularReference);
        }

        var updatedNewOrder = AddAfter(
            relation.ChildId,
            afterPageId,
            newParentId,
            authorId,
            modifyRelationsForPage);

        var updatedOldOrder = Remove(
            relation,
            relation.ParentId,
            authorId,
            modifyRelationsForPage);

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
            UpdatePreviousRelationLinksOnRemove(relationIndex, relations, changedRelations);
            UpdateNextRelationLinksOnRemove(relationIndex, relations, changedRelations);
            CleanupRelationFromCollections(relation, relationIndex, relations);

            UpdateCacheWithModifiedParent(parent, relations);
            PersistRelationChangesToDatabase(relation, changedRelations, modifyRelationsForPage, authorId);
        }

        return relations.ToList();
    }

    private static void UpdateNextRelationLinksOnRemove(int relationIndex, IList<PageRelationCache> relations,
        List<PageRelationCache> changedRelations)
    {
        if (relationIndex < 0 || relationIndex >= relations.Count)
            return;

        var nextRelation = relations[relationIndex + 1];
        nextRelation.PreviousId = relationIndex > 0
            ? relations[relationIndex - 1].ChildId
            : null;

        EntityCache.AddOrUpdate(nextRelation);
        changedRelations.Add(nextRelation);
    }

    private static void UpdatePreviousRelationLinksOnRemove(int relationIndex, IList<PageRelationCache> relations,
        List<PageRelationCache> changedRelations)
    {
        if (relationIndex <= 0)
            return;

        var previousRelation = relations[relationIndex - 1];
        previousRelation.NextId = relationIndex < relations.Count - 1
            ? relations[relationIndex + 1].ChildId
            : null;

        EntityCache.AddOrUpdate(previousRelation);
        changedRelations.Add(previousRelation);
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
            Log.Error(
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

                Log.Error(
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
                Log.Error(
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
        Log.Error("PageRelations - Sort: Broken Link Start - PageId:{0}, RelationId:{1}",
            pageId, sortedRelations.LastOrDefault()?.Id);

        foreach (var childRelation in childRelations)
        {
            if (!addedRelationIds.Contains(childRelation.Id))
            {
                if (!addedChildIds.Contains(childRelation.ChildId))
                    sortedRelations.Add(childRelation);

                Log.Error("PageRelations - Sort: Broken Link - PageId:{0}, RelationId:{1}",
                    pageId, childRelation.Id);
            }
        }

        Log.Error("PageRelations - Sort: Broken Link End - PageId:{0}", pageId);
    }

    private static void CleanupRelationFromCollections(
        PageRelationCache relation,
        int relationIndex,
        IList<PageRelationCache> relations)
    {
        relations.RemoveAt(relationIndex);
        RemoveRelationFromParentRelations(relation);
        EntityCache.Remove(relation);
    }

    private static void UpdateCacheWithModifiedParent(PageCacheItem parent, IList<PageRelationCache> relations)
    {
        parent.ChildRelations = relations;
        EntityCache.AddOrUpdate(parent);
    }

    private static void PersistRelationChangesToDatabase(
        PageRelationCache relation,
        List<PageRelationCache> changedRelations,
        ModifyRelationsForPage modifyRelationsForPage,
        int authorId)
    {
        modifyRelationsForPage.UpdateRelationsInDb(changedRelations, authorId);
        modifyRelationsForPage.DeleteRelationInDb(relation.Id, authorId);
    }
}