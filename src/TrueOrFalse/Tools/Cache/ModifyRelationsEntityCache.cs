
using TrueOrFalse.Utilities.ScheduledJobs;

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

    public static int RemoveParent(CategoryCacheItem childCategory, int parentId, int authorId)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relationToRemove = parent.ChildRelations.Where(r => r.ChildId == childCategory.Id).FirstOrDefault();

        if (relationToRemove != null)
        {
            Remove(relationToRemove, parentId, authorId);
            childCategory.ParentRelations.Remove(relationToRemove);
        }

        return relationToRemove.Id;
    }

    public static CategoryCacheRelation AddChild(int parentId, int childId) => AddChild(EntityCache.GetCategory(parentId), EntityCache.GetCategory(childId));

    public static CategoryCacheRelation AddChild(CategoryCacheItem parent, CategoryCacheItem child)
    {
        var previousId = parent.ChildRelations.LastOrDefault()?.ChildId;
        var newRelation = new CategoryCacheRelation
        {
            ParentId = parent.Id,
            ChildId = child.Id,
            PreviousId = previousId,
            NextId = null
        };
        parent.ChildRelations.Add(newRelation); 
        child.ParentRelations.Add(newRelation);

        return newRelation;
    }

    public static bool CanBeMoved(int childId, int parentId)
    {
        var child = EntityCache.GetCategory(childId);
        var parent = EntityCache.GetCategory(parentId);

        if (GraphService.Descendants(childId).Any(r => r.Id == parentId))
            return false;

        return true;
    }

    public static (List<CategoryCacheRelation> UpdatedOldOrder, List<CategoryCacheRelation> UpdatedNewOrder) MoveBefore(
        CategoryCacheRelation relation,
        int beforeTopicId,
        int newParentId,
        int oldParentId,
        int authorId)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            throw new Exception("circular reference");
        }

        var updatedOldOrder = Remove(relation, oldParentId, authorId);
        var updatedNewOrder = AddBefore(relation.ChildId, beforeTopicId, newParentId, authorId);

        return (updatedOldOrder, updatedNewOrder);
    }

    public static (List<CategoryCacheRelation> UpdatedOldOrder, List<CategoryCacheRelation> UpdatedNewOrder) MoveAfter(
        CategoryCacheRelation relation,
        int afterTopicId,
        int newParentId,
        int oldParentId,
        int authorId)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            throw new Exception("circular reference");
        }

        var updatedOldOrder = Remove(relation, oldParentId, authorId);
        var updatedNewOrder = AddAfter(relation.ChildId, afterTopicId, newParentId, authorId);

        return (updatedOldOrder, updatedNewOrder);
    }

    private static List<CategoryCacheRelation> Remove(CategoryCacheRelation relation, int oldParentId, int authorId)
    {
        var relations = EntityCache.GetCategory(oldParentId).ChildRelations;

        var relationIndex = relations.IndexOf(relation);
        if (relationIndex != -1)
        {
            var changedRelations = new List<CategoryCacheRelation>();

            if (relationIndex > 0)
            {
                var previousRelation = relations[relationIndex - 1];
                previousRelation.NextId = relationIndex < relations.Count - 1 ? relations[relationIndex + 1].ChildId : (int?)null;
                changedRelations.Add(previousRelation);
            }
            if (relationIndex < relations.Count - 1)
            {
                var nextRelation = relations[relationIndex + 1];
                nextRelation.PreviousId = relationIndex > 0 ? relations[relationIndex - 1].ChildId : (int?)null;
                changedRelations.Add(nextRelation);
            }

            relations.RemoveAt(relationIndex);

            JobScheduler.StartImmediately_ModifyTopicRelations(changedRelations, authorId);
        }

        return relations.ToList();
    }

    private static List<CategoryCacheRelation> AddBefore(int topicId, int beforeTopicId, int parentId, int authorId)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relations = parent.ChildRelations.ToList();
        return Insert(topicId, beforeTopicId, parentId, relations, false, authorId);
    }

    private static List<CategoryCacheRelation> AddAfter(int topicId, int afterTopicId, int parentId, int authorId)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relations = parent.ChildRelations.ToList();
        return Insert(topicId, afterTopicId, parentId, relations, true, authorId);
    }

    private static List<CategoryCacheRelation> Insert(int childId, int targetTopicId, int parentId, List<CategoryCacheRelation> relations, bool insertAfter, int authorId)
    {
        var relation = new CategoryCacheRelation
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

        relations.Insert(positionToInsert, relation);

        relation.PreviousId = insertAfter ? targetTopicId : (positionToInsert > 0 ? relations[positionToInsert - 1].ChildId : (int?)null);
        relation.NextId = positionToInsert < relations.Count - 1 ? relations[positionToInsert + 1].ChildId : (int?)null;

        var changedRelations = new List<CategoryCacheRelation>();

        if (positionToInsert > 0 && !insertAfter)
        {
            var previousRelation = relations[positionToInsert - 1];
            previousRelation.NextId = relation.ChildId;
            changedRelations.Add(previousRelation);
        }
        if (positionToInsert < relations.Count - 1 && insertAfter)
        {
            var nextRelation = relations[positionToInsert + 1];
            nextRelation.PreviousId = relation.ChildId;
            changedRelations.Add(nextRelation);
        }

        if (insertAfter)
        {
            relations[targetPosition].NextId = relation.ChildId;
        }
        else if (targetPosition > 0)
        {
            relations[targetPosition - 1].NextId = relation.ChildId;
        }

        changedRelations.Add(relation);

        JobScheduler.StartImmediately_ModifyTopicRelations(changedRelations, authorId);

        return relations;
    }

    public List<CategoryCacheRelation> SortTopics(List<CategoryCacheRelation> categoryRelations)
    {
        var sortedList = new List<CategoryCacheRelation>();
        var addedIds = new HashSet<int>();
        var current = categoryRelations.FirstOrDefault(x => x.PreviousId == null);

        while (current != null)
        {
            sortedList.Add(current);
            addedIds.Add(current.ChildId);
            current = categoryRelations.FirstOrDefault(x => x.ChildId == current.NextId);
        }

        if (sortedList.Count != categoryRelations.Count)
        {
            Logg.r.Error("Topic Order Sort Error");
            foreach (var relation in categoryRelations)
            {
                if (!addedIds.Contains(relation.ChildId))
                {
                    sortedList.Add(relation);
                }
            }
        }

        return sortedList;
    }
}