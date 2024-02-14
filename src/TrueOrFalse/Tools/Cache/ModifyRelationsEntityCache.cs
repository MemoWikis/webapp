
public class ModifyRelationsEntityCache
{
    public static void RemoveRelations(CategoryCacheItem category)
    {
        var allParents = GraphService.Ascendants(category.Id);
        foreach (var parent in allParents)
        {
            for (var i = 0; i < parent.ParentRelations.Count; i++)
            {
                var relation = parent.ParentRelations[i];

                if (relation.ParentId == category.Id)
                {
                    parent.ParentRelations.Remove(relation);
                    break;
                }
            }
        }
    }

    public static void AddParent(int childId, int parentId) => AddParent(EntityCache.GetCategory(childId), parentId);

    public static void AddParent(CategoryCacheItem child, int parentId)
    {
        child.ParentRelations.Add(new CategoryCacheRelation
        {
            ParentId = parentId,
            ChildId = child.Id
        }); 
    }
    public static void RemoveParent(CategoryCacheItem categoryCacheItem, int parentId)
    {
        for (int i = 0; i < categoryCacheItem.ParentRelations.Count; i++)
        {
            var relation = categoryCacheItem.ParentRelations[i];

            if (relation.ChildId == categoryCacheItem.Id &&
                relation.ParentId == parentId)
            {
                categoryCacheItem.ParentRelations.RemoveAt(i);
                break;
            }
        }
    }

    public static void MoveTopic(int childId, int oldParentId, int newParentId)
    {
        var child = EntityCache.GetCategory(childId);
        AddParent(child, newParentId);
        RemoveParent(child, oldParentId);
    }

    public static void AddChild(int parentId, int childId) => AddChild(EntityCache.GetCategory(parentId), EntityCache.GetCategory(childId));

    public static void AddChild(CategoryCacheItem parent, CategoryCacheItem child)
    {
        var previousId = parent.ChildRelations.LastOrDefault()?.ChildId;

        parent.ChildRelations.Add(new CategoryCacheRelation
        {
            ParentId = parent.Id,
            ChildId = child.Id,
            PreviousId = previousId,
            NextId = null
        });

        child.ParentRelations.Add(new CategoryCacheRelation
        {
            ParentId = parent.Id,
            ChildId = child.Id
        });
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
        var relations = parent.ChildRelations.ToList();
        return Insert(topicId, beforeTopicId, parentId, relations, false);
    }

    private List<CategoryCacheRelation> AddAfter(int topicId, int afterTopicId, int parentId)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relations = parent.ChildRelations.ToList();
        return Insert(topicId, afterTopicId, parentId, relations, true);
    }

    private List<CategoryCacheRelation> Insert(int childId, int targetTopicId, int parentId, List<CategoryCacheRelation> relations, bool insertAfter)
    {
        var node = new CategoryCacheRelation
        {
            ChildId = childId,
            ParentId = parentId,
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

        if (positionToInsert > 0 && !insertAfter)
        {
            relations[positionToInsert - 1].NextId = node.ChildId;
        }
        if (positionToInsert < relations.Count - 1 && insertAfter)
        {
            relations[positionToInsert + 1].PreviousId = node.ChildId;
        }
        if (insertAfter)
        {
            relations[targetPosition].NextId = node.ChildId;
        }
        else if (targetPosition > 0)
        {
            relations[targetPosition - 1].NextId = node.ChildId;
        }

        return relations;
    }

    public List<CategoryRelation> SortTopics(List<CategoryCacheRelation> categoryRelations)
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
}