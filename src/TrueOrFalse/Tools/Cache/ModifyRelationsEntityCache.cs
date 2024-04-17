
using System.Security;

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
        {
            Logg.r.Error("CategoryRelations - RemoveParent: No parents remaining - childId:{0}, parentIdToRemove:{1}", childCategory.Id, parentId);
            throw new Exception("No parents remaining");
        }

        if (!permissionCheck.CanEdit(childCategory) && !permissionCheck.CanEdit(parent))
        {
            Logg.r.Error("CategoryRelations - RemoveParent: No rights to edit - childId:{0}, parentId:{1}", childCategory.Id, parentId);
            throw new SecurityException("Not allowed to edit category");
        }

        var relationToRemove = parent?.ChildRelations.FirstOrDefault(r => r.ChildId == childCategory.Id);

        if (relationToRemove != null)
        {
            TopicOrderer.Remove(relationToRemove, parentId, authorId, modifyRelationsForCategory);
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