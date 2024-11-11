using System.Security;

public class ModifyRelationsEntityCache
{
    public static void RemoveRelationsForCategoryDeleter(
        PageCacheItem page,
        int userId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var allRelations = EntityCache.GetCacheRelationsByPageId(page.Id);
        foreach (var relation in allRelations)
        {
            if (relation.ChildId == page.Id)
            {
                var parent = EntityCache.GetPage(relation.ParentId);
                RemoveParent(page, parent, userId, modifyRelationsForCategory);
            }
            else
            {
                var child = EntityCache.GetPage(relation.ChildId);
                RemoveParent(child, page, userId, modifyRelationsForCategory);
            }
        }
    }

    private static bool CheckParentAvailability(
        IEnumerable<PageCacheItem> parentCategories,
        PageCacheItem childPage)
    {
        var allParentsArePrivate =
            parentCategories.All(c => c.Visibility != PageVisibility.All);
        var childIsPublic = childPage.Visibility == PageVisibility.All;

        if (!parentCategories.Any() || allParentsArePrivate && childIsPublic)
            return false;

        return true;
    }

    public static bool RemoveParent(
        PageCacheItem childPage,
        int parentId,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory,
        PermissionCheck permissionCheck)
    {
        var parent = EntityCache.GetPage(parentId);

        var newParentRelationsIds = childPage.ParentRelations.Where(r => r.ParentId != parentId)
            .Select(r => r.ParentId);
        var parentCategories = EntityCache.GetPages(newParentRelationsIds);

        if (!childPage.IsStartPage() &&
            !CheckParentAvailability(parentCategories, childPage))
        {
            Logg.r.Error(
                "CategoryRelations - RemoveParent: No parents remaining - childId:{0}, parentIdToRemove:{1}",
                childPage.Id, parentId);
            throw new Exception("No parents remaining");
        }

        if (!permissionCheck.CanEdit(childPage) && !permissionCheck.CanEdit(parent))
        {
            Logg.r.Error(
                "CategoryRelations - RemoveParent: No rights to edit - childId:{0}, parentId:{1}",
                childPage.Id, parentId);
            throw new SecurityException("Not allowed to edit category");
        }

        return RemoveParent(childPage, parent, authorId, modifyRelationsForCategory);
    }

    private static bool RemoveParent(
        PageCacheItem childPage,
        PageCacheItem parent,
        int authorId,
        ModifyRelationsForCategory modifyRelationsForCategory)
    {
        var relationToRemove =
            parent.ChildRelations.FirstOrDefault(r => r.ChildId == childPage.Id);

        if (relationToRemove != null)
        {
            PageOrderer.Remove(relationToRemove, parent.Id, authorId, modifyRelationsForCategory);
            childPage.ParentRelations.Remove(relationToRemove);
            return true;
        }

        return false;
    }

    public static PageRelationCache AddChild(PageRelation pageRelation)
    {
        var newRelation = PageRelationCache.ToCategoryCacheRelation(pageRelation);

        EntityCache.GetPage(newRelation.ParentId)?.ChildRelations.Add(newRelation);
        EntityCache.GetPage(newRelation.ChildId)?.ParentRelations.Add(newRelation);
        EntityCache.AddOrUpdate(newRelation);

        return newRelation;
    }
}