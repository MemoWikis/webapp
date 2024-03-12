
using TrueOrFalse.Utilities.ScheduledJobs;

public class TopicOrderService
{
    private readonly CategoryRelationRepo _categoryRelationRepo;
    private readonly CategoryRepository _categoryRepo;

    public TopicOrderService(CategoryRelationRepo categoryRelationRepo, CategoryRepository categoryRepo)
    {
        _categoryRelationRepo = categoryRelationRepo;
        _categoryRepo = categoryRepo;
    }

    public (List<CategoryCacheRelation> UpdatedOldOrder, List<CategoryCacheRelation> UpdatedNewOrder) MoveBefore(
        CategoryCacheRelation relation, 
        int beforeTopicId,
        int newParentId,
        int oldParentId)
    {
        var updatedNewOrder = AddBefore(relation.ChildId, beforeTopicId, newParentId);
        var updatedOldOrder = Remove(relation, oldParentId);

        return (updatedOldOrder, updatedNewOrder);
    }

    public (List<CategoryCacheRelation> UpdatedOldOrder, List<CategoryCacheRelation> UpdatedNewOrder) MoveAfter(
        CategoryCacheRelation relation,
        int afterTopicId,
        int newParentId,
        int oldParentId)
    {
        var updatedNewOrder = AddAfter(relation.ChildId, afterTopicId, newParentId);
        var updatedOldOrder = Remove(relation, oldParentId);

        return (updatedOldOrder, updatedNewOrder);
    }

    private List<CategoryCacheRelation> Remove(CategoryCacheRelation relation, int oldParentId)
    {
        var relations = EntityCache.GetCategory(oldParentId).ParentRelations;

        var nodeIndex = relations.IndexOf(relation);
        if (nodeIndex != -1)
        {
            if (nodeIndex > 0)
            {
                relations[nodeIndex - 1].NextId = nodeIndex < relations.Count - 1 ? relations[nodeIndex + 1].ChildId : (int?)null;
            }
            if (nodeIndex < relations.Count - 1)
            {
                relations[nodeIndex + 1].PreviousId = nodeIndex > 0 ? relations[nodeIndex - 1].ChildId : (int?)null;
            }
            relations.RemoveAt(nodeIndex);
        }

        return relations.ToList();
    }

    private List<CategoryCacheRelation> AddBefore(int topicId, int beforeTopicId, int parentId)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relations = parent.ParentRelations.ToList();
        var child = EntityCache.GetCategory(topicId);
        return Insert(child, beforeTopicId, parent, relations, false);
    }

    public List<CategoryCacheRelation> AddAfter(int topicId, int afterTopicId, int parentId)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relations = parent.ParentRelations.ToList();
        var child = EntityCache.GetCategory(topicId);
        return Insert(child, afterTopicId, parent, relations, true);
    }

    private List<CategoryCacheRelation> Insert(CategoryCacheItem child, int targetTopicId, CategoryCacheItem parent, List<CategoryCacheRelation> relations, bool insertAfter)
    {
        var node = new CategoryCacheRelation
        {
            ChildId = child.Id,
            ParentId = parent.Id,
            PreviousId = insertAfter ? targetTopicId : null,
            NextId = !insertAfter ? targetTopicId : null,
        };
        int targetPosition = relations.FindIndex(n => n.ChildId == targetTopicId);
        if (targetPosition == -1)
        {
            throw new InvalidOperationException("Target node not found in the order.");
        }

        int positionToInsert = insertAfter ? targetPosition + 1 : targetPosition;

        relations.Insert(positionToInsert, node);

        node.PreviousId = insertAfter ? targetTopicId : (positionToInsert > 0 ? relations[positionToInsert - 1].ChildId : (int?)null);
        node.NextId = positionToInsert < relations.Count - 1 ? relations[positionToInsert + 1].ChildId : (int?)null;

        var relationsToUpdateOrAdd = new List<CategoryCacheRelation>();

        if (positionToInsert > 0 && !insertAfter)
        {
            relations[positionToInsert - 1].NextId = node.ChildId;
            relationsToUpdateOrAdd.Add(relations[positionToInsert - 1]);
        }
        if (positionToInsert < relations.Count - 1 && insertAfter)
        {
            relations[positionToInsert + 1].PreviousId = node.ChildId;
            relationsToUpdateOrAdd.Add(relations[positionToInsert + 1]);
        }
        if (insertAfter)
        {
            relations[targetPosition].NextId = node.ChildId;
        }
        else if (targetPosition > 0)
        {
            relations[targetPosition - 1].NextId = node.ChildId;
        }

        relationsToUpdateOrAdd.Add(relations[targetPosition]);
        //update nodes

        return relations;
    }

    public List<CategoryCacheRelation> SortTopics(List<CategoryCacheRelation> categoryRelations)
    {
        var sortedList = new List<CategoryCacheRelation>();
        var current = categoryRelations.FirstOrDefault(x => x.PreviousId == null);

        while (current != null)
        {
            sortedList.Add(current);
            current = categoryRelations.FirstOrDefault(x => x.ChildId == current.NextId);
        }

        return sortedList;
    }

    private void UpdateRelations(List<CategoryCacheRelation> relations)
    {
        JobScheduler.StartImmediately_ModifyTopicRelations(relations, _sessionUser.UserId);
    }
}