using System.Security;

public class ModifyRelationsEntityCache
{
    public static void RemoveRelationsForCategoryDeleter(
        CategoryCacheItem category,
        int userId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var allRelations = EntityCache.GetCacheRelationsByTopicId(category.Id);
        foreach (var relation in allRelations)
        {
            if (relation.ChildId == category.Id)
            {
                var parent = EntityCache.GetCategory(relation.ParentId);
                RemoveParent(category, parent, userId, modifyRelationsForCategory);
            }
            else
            {
                var child = EntityCache.GetCategory(relation.ChildId);
                RemoveParent(child, category, userId, modifyRelationsForCategory);
            }
        }
    }

    private static bool CheckParentAvailability(
        IEnumerable<CategoryCacheItem> parentCategories,
        CategoryCacheItem childCategory)
    {
        var allParentsArePrivate =
            parentCategories.All(c => c.Visibility != CategoryVisibility.All);
        var childIsPublic = childCategory.Visibility == CategoryVisibility.All;

        if (!parentCategories.Any() || allParentsArePrivate && childIsPublic)
            return false;

        return true;
    }

    public static bool RemoveParent(
        CategoryCacheItem childCategory,
        int parentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory,
        PermissionCheck permissionCheck)
    {
        var parent = EntityCache.GetCategory(parentId);

        var newParentRelationsIds = childCategory.ParentRelations.Where(r => r.ParentId != parentId)
            .Select(r => r.ParentId);
        var parentCategories = EntityCache.GetCategories(newParentRelationsIds);

        if (!childCategory.IsStartPage() &&
            !CheckParentAvailability(parentCategories, childCategory))
        {
            Logg.r.Error(
                "CategoryRelations - RemoveParent: No parents remaining - childId:{0}, parentIdToRemove:{1}",
                childCategory.Id, parentId);
            throw new Exception("No parents remaining");
        }

        if (!permissionCheck.CanEdit(childCategory) && !permissionCheck.CanEdit(parent))
        {
            Logg.r.Error(
                "CategoryRelations - RemoveParent: No rights to edit - childId:{0}, parentId:{1}",
                childCategory.Id, parentId);
            throw new SecurityException("Not allowed to edit category");
        }

        return RemoveParent(childCategory, parent, authorId, modifyRelationsForCategory);
    }

    private static bool RemoveParent(
        CategoryCacheItem childCategory,
        CategoryCacheItem parent,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var relationToRemove =
            parent.ChildRelations.FirstOrDefault(r => r.ChildId == childCategory.Id);

        if (relationToRemove != null)
        {
            TopicOrderer.Remove(relationToRemove, parent.Id, authorId, modifyRelationsForCategory);
            childCategory.ParentRelations.Remove(relationToRemove);
            return true;
        }

        return false;
    }

    public static CategoryCacheRelation AddChild(CategoryRelation categoryRelation)
    {
        var newRelation = CategoryCacheRelation.ToCategoryCacheRelation(categoryRelation);

        EntityCache.GetCategory(newRelation.ParentId)?.ChildRelations.Add(newRelation);
        EntityCache.GetCategory(newRelation.ChildId)?.ParentRelations.Add(newRelation);
        EntityCache.AddOrUpdate(newRelation);

        return newRelation;
    }
}