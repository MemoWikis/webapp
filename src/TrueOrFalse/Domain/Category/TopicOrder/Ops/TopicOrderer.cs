using TrueOrFalse.Utilities.ScheduledJobs;

public class TopicOrderer
{
    public static bool CanBeMoved(int childId, int parentId)
    {
        if (childId == parentId)
            return false;

        if (GraphService.Descendants(childId).Any(r => r.Id == parentId))
            return false;

        return true;
    }

    public static async Task MoveInAsync(
        CategoryCacheRelation relation,
        int newParentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory,
        PermissionCheck permissionCheck)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            Logg.r.Error(
                "CategoryRelations - moveIn: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Category.CircularReference);
        }

        await modifyRelationsForCategory
            .AddChildAsync(newParentId, relation.ChildId)
            .ConfigureAwait(false);
        ModifyRelationsEntityCache.RemoveParentAsync(EntityCache.GetCategory(relation.ChildId),
            relation.ParentId, authorId, modifyRelationsForCategory, permissionCheck);
    }

    public static async Task<(
        List<CategoryCacheRelation> UpdatedOldOrder,
        List<CategoryCacheRelation> UpdatedNewOrder)> MoveBeforeAsync(
        CategoryCacheRelation relation,
        int beforeTopicId,
        int newParentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            Logg.r.Error(
                "CategoryRelations - MoveBeforeAsync: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Category.CircularReference);
        }

        var updatedNewOrder =
            await AddBeforeAsync(
                    relation.ChildId,
                    beforeTopicId,
                    newParentId,
                    authorId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);
        var updatedOldOrder =
            await RemoveAsync(
                    relation,
                    relation.ParentId,
                    authorId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);

        return (updatedOldOrder, updatedNewOrder);
    }

    public static async Task<(
        List<CategoryCacheRelation> UpdatedOldOrder,
        List<CategoryCacheRelation> UpdatedNewOrder)> MoveAfterAsync(
        CategoryCacheRelation relation,
        int afterTopicId,
        int newParentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            Logg.r.Error(
                "CategoryRelations - MoveAfterAsync: circular reference - childId:{0}, parentId:{1}",
                relation.ChildId, newParentId);
            throw new Exception(FrontendMessageKeys.Error.Category.CircularReference);
        }

        var updatedNewOrder =
            await AddAfterAsync(
                    relation.ChildId,
                    afterTopicId,
                    newParentId,
                    authorId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);

        var updatedOldOrder =
            await RemoveAsync(
                    relation,
                    relation.ParentId,
                    authorId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);

        return (updatedOldOrder, updatedNewOrder);
    }

    public static async Task<List<CategoryCacheRelation>> RemoveAsync(
        CategoryCacheRelation relation,
        int oldParentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var relations = EntityCache.GetCategory(oldParentId).ChildRelations;

        var relationIndex = relations.IndexOf(relation);
        if (relationIndex != -1)
        {
            var changedRelations = new List<CategoryCacheRelation>();

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
            await modifyRelationsForCategory
                .UpdateRelationsInDbAsync(changedRelations, authorId)
                .ConfigureAwait(false);
            await modifyRelationsForCategory
                .DeleteRelationInDbAsync(relation.Id, authorId)
                .ConfigureAwait(false);
        }

        return relations.ToList();
    }

    private static void RemoveRelationFromParentRelations(CategoryCacheRelation relation)
    {
        var child = EntityCache.GetCategory(relation.ChildId);
        var relations = child.ParentRelations;

        var relationIndex = relations.IndexOf(relation);
        if (relationIndex >= 0)
            relations.RemoveAt(relationIndex);
    }

    private static async Task<List<CategoryCacheRelation>> AddBeforeAsync(
        int topicId,
        int beforeTopicId,
        int parentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var parent = EntityCache.GetCategory(parentId);

        var relations = parent.ChildRelations.ToList();
        var newRelations =
            await InsertAsync(
                    topicId,
                    beforeTopicId,
                    parentId,
                    relations,
                    false,
                    authorId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);

        parent.ChildRelations = newRelations;
        return newRelations;
    }

    private static async Task<List<CategoryCacheRelation>> AddAfterAsync(
        int topicId,
        int afterTopicId,
        int parentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var parent = EntityCache.GetCategory(parentId);

        var relations = parent.ChildRelations.ToList();
        var newRelations =
            await InsertAsync(
                    topicId,
                    afterTopicId,
                    parentId,
                    relations,
                    true,
                    authorId,
                    modifyRelationsForCategory)
                .ConfigureAwait(false);

        parent.ChildRelations = newRelations;
        return newRelations;
    }

    private static async Task<List<CategoryCacheRelation>> InsertAsync(
        int childId,
        int targetTopicId,
        int parentId,
        List<CategoryCacheRelation> relations,
        bool insertAfter,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var relation = new CategoryCacheRelation
        {
            ChildId = childId,
            ParentId = parentId,
            PreviousId = insertAfter ? targetTopicId : null,
            NextId = !insertAfter ? targetTopicId : null,
        };

        var targetPosition = relations.FindIndex(r => r.ChildId == targetTopicId);
        if (targetPosition == -1)
        {
            Logg.r.Error(
                "CategoryRelations - InsertAsync: Targetposition not found - parentId:{0}, targetTopicId:{1}",
                parentId, targetTopicId);
            throw new InvalidOperationException(FrontendMessageKeys.Error.Default);
        }

        var positionToInsert = insertAfter ? targetPosition + 1 : targetPosition;

        relations.Insert(positionToInsert, relation);

        var currentRelation = relations[positionToInsert];
        var changedRelations = new List<CategoryCacheRelation>();

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

        var newRelationId = modifyRelationsForCategory.CreateNewRelationAndGetId(
            currentRelation.ParentId, currentRelation.ChildId,
            currentRelation.NextId, currentRelation.PreviousId);

        currentRelation.Id = newRelationId;

        EntityCache.AddOrUpdate(currentRelation);
        await modifyRelationsForCategory
            .UpdateRelationsInDbAsync(changedRelations, authorId)
            .ConfigureAwait(false);

        return relations;
    }

    private static void InsertBefore(
        List<CategoryCacheRelation> relations,
        int positionToInsert,
        CategoryCacheRelation currentRelation,
        List<CategoryCacheRelation> changedRelations)
    {
        var nextRelation = relations[positionToInsert + 1];
        nextRelation.PreviousId = currentRelation.ChildId;

        EntityCache.AddOrUpdate(nextRelation);
        changedRelations.Add(nextRelation);

        if (positionToInsert > 0) // updates the relation before the newly inserted relation
        {
            var previousRelation = relations[positionToInsert - 1];
            previousRelation.NextId = currentRelation.ChildId;
            currentRelation.PreviousId = previousRelation.ChildId;

            EntityCache.AddOrUpdate(previousRelation);
            changedRelations.Add(previousRelation);
        }
    }

    private static void InsertAfter(
        List<CategoryCacheRelation> relations,
        int targetPosition,
        CategoryCacheRelation currentRelation,
        List<CategoryCacheRelation> changedRelations,
        int positionToInsert)
    {
        var previousRelation = relations[targetPosition];
        previousRelation.NextId = currentRelation.ChildId;

        EntityCache.AddOrUpdate(previousRelation);
        changedRelations.Add(previousRelation);

        if (positionToInsert + 1 <
            relations.Count) // updates the relation after the newly inserted relation
        {
            var nextRelation = relations[positionToInsert + 1];
            nextRelation.PreviousId = currentRelation.ChildId;
            currentRelation.NextId = nextRelation.ChildId;

            EntityCache.AddOrUpdate(nextRelation);
            changedRelations.Add(nextRelation);
        }
    }

    public static IList<CategoryCacheRelation> Sort(int topicId)
    {
        var childRelations = EntityCache.GetChildRelationsByParentId(topicId);
        return childRelations.Count <= 0 ? childRelations : Sort(childRelations, topicId);
    }

    public static IList<CategoryCacheRelation> Sort(
        IList<CategoryCacheRelation> childRelations,
        int topicId)
    {
        var currentRelation = childRelations.FirstOrDefault(r => r.PreviousId == null);
        var sortedRelations = new List<CategoryCacheRelation>();
        var addedRelationIds = new HashSet<int>();

        while (currentRelation != null)
        {
            sortedRelations.Add(currentRelation);
            addedRelationIds.Add(currentRelation.Id);

            currentRelation =
                childRelations.FirstOrDefault(r => r.ChildId == currentRelation.NextId);

            if (sortedRelations.Count >= childRelations.Count && currentRelation != null)
            {
                Logg.r.Error(
                    "CategoryRelations - Sort: Force break 'while loop', faulty links - TopicId:{0}",
                    topicId);
                break;
            }
        }

        if (sortedRelations.Count < childRelations.Count)
            AppendMissingRelations(topicId, sortedRelations, childRelations, addedRelationIds);

        return sortedRelations;
    }

    private static void AppendMissingRelations(
        int topicId,
        List<CategoryCacheRelation> sortedRelations,
        IList<CategoryCacheRelation> childRelations,
        HashSet<int> addedRelationIds)
    {
        Logg.r.Error("CategoryRelations - Sort: Broken Link Start - TopicId:{0}, RelationId:{1}",
            topicId, sortedRelations.LastOrDefault()?.Id);

        foreach (var childRelation in childRelations)
        {
            if (!addedRelationIds.Contains(childRelation.Id))
            {
                sortedRelations.Add(childRelation);
                Logg.r.Error("CategoryRelations - Sort: Broken Link - TopicId:{0}, RelationId:{1}",
                    topicId, childRelation.Id);
            }
        }

        Logg.r.Error("CategoryRelations - Sort: Broken Link End - TopicId:{0}", topicId);
    }
}