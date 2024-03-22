
using System.Security;
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

    private static bool CheckParentAvailability(IEnumerable<CategoryCacheItem> parentCategories, CategoryCacheItem childCategory)
    {
        var allParentsArePrivate = parentCategories.All(c => c.Visibility != CategoryVisibility.All);
        var childIsPublic = childCategory.Visibility == CategoryVisibility.All;

        if (!parentCategories.Any() || allParentsArePrivate && childIsPublic)
            return false;

        return true;
    }

    public static bool RemoveParent(CategoryCacheItem childCategory, int parentId, int authorId, ModifyRelationsForCategory modifyRelationsForCategory, PermissionCheck permissionCheck)
    {
        var parent = EntityCache.GetCategory(parentId);

        var newParentRelationsIds = childCategory.ParentRelations.Where(r => r.ParentId != parentId).Select(r => r.ParentId);
        var parentCategories = EntityCache.GetCategories(newParentRelationsIds);

        if (!childCategory.IsStartPage() && !CheckParentAvailability(parentCategories, childCategory))
            throw new Exception("No parents remaining");

        if (!permissionCheck.CanEdit(childCategory) && !permissionCheck.CanEdit(parent))
            throw new SecurityException("Not allowed to edit category");

        var relationToRemove = parent.ChildRelations.Where(r => r.ChildId == childCategory.Id).FirstOrDefault();

        if (relationToRemove != null)
        {
            Remove(relationToRemove, parentId, authorId, modifyRelationsForCategory);
            childCategory.ParentRelations.Remove(relationToRemove);
            return true;
        }

        return false;
    }

    public static CategoryCacheRelation AddChild(CategoryRelation categoryRelation)
    {
        var newRelation = CategoryCacheRelation.ToCategoryCacheRelation(categoryRelation);
        EntityCache.AddOrUpdate(newRelation);

        EntityCache.GetCategory(newRelation.ParentId)?.ChildRelations.Add(newRelation);
        EntityCache.GetCategory(newRelation.ChildId)?.ParentRelations.Add(newRelation);

        return newRelation;
    }

    public static bool CanBeMoved(int childId, int parentId)
    {
        if (childId == parentId) 
            return false;

        if (GraphService.Descendants(childId).Any(r => r.Id == parentId))
            return false;

        return true;
    }

    public static void MoveIn(
        CategoryCacheRelation relation,
        int newParentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory, PermissionCheck permissionCheck)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            throw new Exception("circular reference");
        }

        modifyRelationsForCategory.AddChild(newParentId, relation.ParentId);
        RemoveParent(EntityCache.GetCategory(relation.ChildId), relation.ParentId, authorId, modifyRelationsForCategory, permissionCheck);
    }

    public static (List<CategoryCacheRelation> UpdatedOldOrder, List<CategoryCacheRelation> UpdatedNewOrder) MoveBefore(
        CategoryCacheRelation relation,
        int beforeTopicId,
        int newParentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            throw new Exception("circular reference");
        }

        var updatedNewOrder = AddBefore(relation.ChildId, beforeTopicId, newParentId, authorId, modifyRelationsForCategory);
        var updatedOldOrder = Remove(relation, relation.ParentId, authorId, modifyRelationsForCategory);

        return (updatedOldOrder, updatedNewOrder);
    }

    public static (List<CategoryCacheRelation> UpdatedOldOrder, List<CategoryCacheRelation> UpdatedNewOrder) MoveAfter(
        CategoryCacheRelation relation,
        int afterTopicId,
        int newParentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        if (!CanBeMoved(relation.ChildId, newParentId))
        {
            throw new Exception("circular reference");
        }

        var updatedNewOrder = AddAfter(relation.ChildId, afterTopicId, newParentId, authorId, modifyRelationsForCategory);
        var updatedOldOrder = Remove(relation, relation.ParentId, authorId, modifyRelationsForCategory);

        return (updatedOldOrder, updatedNewOrder);
    }

    private static List<CategoryCacheRelation> Remove(CategoryCacheRelation relation, int oldParentId, int authorId, ModifyRelationsForCategory modifyRelationsForCategory)
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

                EntityCache.AddOrUpdate(previousRelation);
                changedRelations.Add(previousRelation);
            }
            if (relationIndex < relations.Count - 1)
            {
                var nextRelation = relations[relationIndex + 1];
                nextRelation.PreviousId = relationIndex > 0 ? relations[relationIndex - 1].ChildId : (int?)null;

                EntityCache.AddOrUpdate(nextRelation);
                changedRelations.Add(nextRelation);
            }

            relations.RemoveAt(relationIndex);
            RemoveRelationFromParentRelations(relation);
            //JobScheduler.StartImmediately_ModifyTopicRelations(changedRelations, authorId);
            modifyRelationsForCategory.UpdateRelationsInDb(changedRelations, authorId);

            JobScheduler.StartImmediately_DeleteRelation(relation.Id, authorId);

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

    private static List<CategoryCacheRelation> AddBefore(int topicId, int beforeTopicId, int parentId, int authorId, ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relations = parent.ChildRelations.ToList();
        var newRelations = Insert(topicId, beforeTopicId, parentId, relations, false, authorId, modifyRelationsForCategory);
        parent.ChildRelations = newRelations;
        return newRelations;
    }

    private static List<CategoryCacheRelation> AddAfter(int topicId, int afterTopicId, int parentId, int authorId, ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var parent = EntityCache.GetCategory(parentId);
        var relations = parent.ChildRelations.ToList();
        var newRelations = Insert(topicId, afterTopicId, parentId, relations, true, authorId, modifyRelationsForCategory);
        parent.ChildRelations = newRelations;
        return newRelations;
    }

    private static List<CategoryCacheRelation> Insert(
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
        var targetPosition = relations.FindIndex(n => n.ChildId == targetTopicId);
        if (targetPosition == -1)
        {
            throw new InvalidOperationException("Target node not found in the order.");
        }

        var positionToInsert = insertAfter ? targetPosition + 1 : targetPosition;

        relations.Insert(positionToInsert, relation);

        var currentRelation = relations[positionToInsert];
        var changedRelations = new List<CategoryCacheRelation>();

        if (insertAfter)
        {
            var previousRelation = relations[targetPosition];
            previousRelation.NextId = currentRelation.ChildId;

            EntityCache.AddOrUpdate(previousRelation);
            changedRelations.Add(previousRelation);

            if (positionToInsert + 1 < relations.Count) // updates the relation after the newly inserted relation
            {
                var nextRelation = relations[positionToInsert + 1];
                nextRelation.PreviousId = currentRelation.ChildId;
                currentRelation.NextId = nextRelation.ChildId;

                EntityCache.AddOrUpdate(nextRelation);
                changedRelations.Add(nextRelation);
            }
        }
        else
        {
            var nextRelation = relations[positionToInsert + 1];
            nextRelation.PreviousId = currentRelation.ChildId;

            EntityCache.AddOrUpdate(nextRelation);
            changedRelations.Add(nextRelation);

            if (positionToInsert > 0)  // updates the relation before the newly inserted relation
            {
                var previousRelation = relations[positionToInsert - 1];
                previousRelation.NextId = currentRelation.ChildId;
                currentRelation.PreviousId = previousRelation.ChildId;

                EntityCache.AddOrUpdate(previousRelation);
                changedRelations.Add(previousRelation);
            }
        }

        var newRelationId = modifyRelationsForCategory.CreateNewRelationAndGetId(currentRelation.ParentId, currentRelation.ChildId,
            currentRelation.NextId, currentRelation.PreviousId);

        currentRelation.Id = newRelationId;

        EntityCache.AddOrUpdate(currentRelation);
        modifyRelationsForCategory.UpdateRelationsInDb(changedRelations, authorId);
        //JobScheduler.StartImmediately_ModifyTopicRelations(changedRelations, authorId);

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