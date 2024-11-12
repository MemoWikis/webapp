using System.Security;

public class ModifyRelationsEntityCache
{
    public static void RemoveRelationsForPageDeleter(
        PageCacheItem page,
        int userId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var allRelations = EntityCache.GetCacheRelationsByPageId(page.Id);
        foreach (var relation in allRelations)
        {
            if (relation.ChildId == page.Id)
            {
                var parent = EntityCache.GetPage(relation.ParentId);
                RemoveParent(page, parent, userId, modifyRelationsForPage);
            }
            else
            {
                var child = EntityCache.GetPage(relation.ChildId);
                RemoveParent(child, page, userId, modifyRelationsForPage);
            }
        }
    }

    private static bool CheckParentAvailability(
        IEnumerable<PageCacheItem> parentPages,
        PageCacheItem childPage)
    {
        var allParentsArePrivate =
            parentPages.All(c => c.Visibility != PageVisibility.All);
        var childIsPublic = childPage.Visibility == PageVisibility.All;

        if (!parentPages.Any() || allParentsArePrivate && childIsPublic)
            return false;

        return true;
    }

    public static bool RemoveParent(
        PageCacheItem childPage,
        int parentId,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage,
        PermissionCheck permissionCheck)
    {
        var parent = EntityCache.GetPage(parentId);

        var newParentRelationsIds = childPage.ParentRelations.Where(r => r.ParentId != parentId)
            .Select(r => r.ParentId);
        var parentPages = EntityCache.GetPages(newParentRelationsIds);

        if (!childPage.IsStartPage() &&
            !CheckParentAvailability(parentPages, childPage))
        {
            Logg.r.Error(
                "PageRelations - RemoveParent: No parents remaining - childId:{0}, parentIdToRemove:{1}",
                childPage.Id, parentId);
            throw new Exception("No parents remaining");
        }

        if (!permissionCheck.CanEdit(childPage) && !permissionCheck.CanEdit(parent))
        {
            Logg.r.Error(
                "PageRelations - RemoveParent: No rights to edit - childId:{0}, parentId:{1}",
                childPage.Id, parentId);
            throw new SecurityException("Not allowed to edit category");
        }

        return RemoveParent(childPage, parent, authorId, modifyRelationsForPage);
    }

    private static bool RemoveParent(
        PageCacheItem childPage,
        PageCacheItem parent,
        int authorId,
        ModifyRelationsForPage modifyRelationsForPage)
    {
        var relationToRemove =
            parent.ChildRelations.FirstOrDefault(r => r.ChildId == childPage.Id);

        if (relationToRemove != null)
        {
            PageOrderer.Remove(relationToRemove, parent.Id, authorId, modifyRelationsForPage);
            childPage.ParentRelations.Remove(relationToRemove);
            return true;
        }

        return false;
    }

    public static PageRelationCache AddChild(PageRelation pageRelation)
    {
        var newRelation = PageRelationCache.ToPageCacheRelation(pageRelation);

        EntityCache.GetPage(newRelation.ParentId)?.ChildRelations.Add(newRelation);
        EntityCache.GetPage(newRelation.ChildId)?.ParentRelations.Add(newRelation);
        EntityCache.AddOrUpdate(newRelation);

        return newRelation;
    }
}