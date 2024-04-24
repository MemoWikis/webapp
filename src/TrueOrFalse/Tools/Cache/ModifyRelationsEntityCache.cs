using System.Security;

public class ModifyRelationsEntityCache
{
    public static async Task RemoveRelationsForCategoryDeleter(
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
                await RemoveParentAsync(category, parent, userId, modifyRelationsForCategory)
                    .ConfigureAwait(false);
            }
            else
            {
                var child = EntityCache.GetCategory(relation.ChildId);
                await RemoveParentAsync(child, category, userId, modifyRelationsForCategory)
                    .ConfigureAwait(false);
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

    public static async Task<bool> RemoveParentAsync(
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
                "CategoryRelations - RemoveParentAsync: No parents remaining - childId:{0}, parentIdToRemove:{1}",
                childCategory.Id, parentId);
            throw new Exception("No parents remaining");
        }

        if (!permissionCheck.CanEdit(childCategory) && !permissionCheck.CanEdit(parent))
        {
            Logg.r.Error(
                "CategoryRelations - RemoveParentAsync: No rights to edit - childId:{0}, parentId:{1}",
                childCategory.Id, parentId);
            throw new SecurityException("Not allowed to edit category");
        }

        return await RemoveParentAsync(childCategory, parent, authorId, modifyRelationsForCategory)
            .ConfigureAwait(false);
    }

    private static async Task<bool> RemoveParentAsync(
        CategoryCacheItem childCategory,
        CategoryCacheItem parent,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var relationToRemove =
            parent.ChildRelations.FirstOrDefault(r => r.ChildId == childCategory.Id);

        if (relationToRemove != null)
        {
            await TopicOrderer
                .RemoveAsync(relationToRemove, parent.Id, authorId, modifyRelationsForCategory)
                .ConfigureAwait(false);
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

        return newRelation;
    }
}