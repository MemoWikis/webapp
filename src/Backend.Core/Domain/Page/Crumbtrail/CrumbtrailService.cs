public class CrumbtrailService(
    PermissionCheck _permissionCheck,
    ExtendedUserCache _extendedUserCache,
    SessionUser _sessionUser) : IRegisterAsInstancePerLifetime
{
    public Crumbtrail BuildCrumbtrail(PageCacheItem page, PageCacheItem root)
    {
        var result = new Crumbtrail(page, root);

        if (!page.IsWikiType())
        {
            var parents = page.Parents();
            var rootWikiParent = parents.FirstOrDefault(c => c == root);
            parents = OrderParentList(parents, root.Creator.Id);

            if (rootWikiParent != null)
                AddBreadcrumbParent(result, rootWikiParent, root);
            else
                AddBreadcrumbParents(result, parents, root);

            CheckBreadcrumbTrailCorrectness(result.Items, page);
        }

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private void CheckBreadcrumbTrailCorrectness(IList<CrumbtrailItem> crumbtrailItems, PageCacheItem page)
    {
        if (crumbtrailItems.Count == 0)
            return;

        if (page.Parents().All(c => c.Id != crumbtrailItems[0].Page.Id))
            Log.Error(
                "Breadcrumb - {currentPageId}: next item is not a direct parent, currentItemId: {pageId}, nextItemId: {nextItemId}",
                page.Id, page.Id, crumbtrailItems[0].Page.Id);

        for (int i = 0; i < crumbtrailItems.Count - 1; i++)
        {
            var pageCacheItem = crumbtrailItems[i].Page;
            var nextItemId = crumbtrailItems[i + 1].Page.Id;

            if (!_permissionCheck.CanView(pageCacheItem))
                Log.Error("Breadcrumb - {currentPageId}: visibility/permission", page.Id);

            if (pageCacheItem.Parents().All(c => c.Id != nextItemId))
                Log.Error(
                    "Breadcrumb - {currentPageId}: next item is not a direct parent, currentItemId: {pageid}, nextItemId: {nextItemId}",
                    page.Id, pageCacheItem.Id, nextItemId);
        }
    }

    private void AddBreadcrumbParents(Crumbtrail crumbtrail, IList<PageCacheItem> parents, PageCacheItem root)
    {
        foreach (var parent in parents)
        {
            if (crumbtrail.Rootfound)
                break;

            if (IsLinkedToRoot(parent, root))
                AddBreadcrumbParent(crumbtrail, parent, root);
        }
    }

    private bool IsLinkedToRoot(PageCacheItem page, PageCacheItem root) =>
        GraphService
            .VisibleAscendants(page.Id, _permissionCheck)
            .Any(c => c == root);


    private void AddBreadcrumbParent(Crumbtrail crumbtrail, PageCacheItem pageCacheItem, PageCacheItem root)
    {
        if (_permissionCheck.CanView(pageCacheItem) == false)
            return;

        crumbtrail.Add(pageCacheItem);

        if (root == pageCacheItem)
            return;

        var parents = GraphService.VisibleParents(pageCacheItem.Id, _permissionCheck);
        parents = OrderParentList(parents, root.Creator.Id);

        if (parents.Any(c => c.Id == root.Id))
            crumbtrail.Add(root);
        else
            AddBreadcrumbParents(crumbtrail, parents, root);
    }

    private static List<PageCacheItem> OrderParentList(IList<PageCacheItem> parents, int wikiCreatorId)
    {
        var parentsWithWikiCreator = parents.Where(c => c.Creator.Id == wikiCreatorId);
        var parentsWithoutWikiCreator = parents.Where(c => c.Creator.Id != wikiCreatorId);

        var orderedParents = new List<PageCacheItem>();
        orderedParents.AddRange(parentsWithWikiCreator);
        orderedParents.AddRange(parentsWithoutWikiCreator);

        return orderedParents;
    }

    public PageCacheItem GetWiki(PageCacheItem pageCacheItem, SessionUser sessionUser)
    {
        var currentWikiId = sessionUser.CurrentWikiId;

        if (pageCacheItem.IsWikiType())
            return pageCacheItem;

        var parents = GraphService.VisibleAscendants(pageCacheItem.Id, _permissionCheck);
        if (!IsCurrentWikiValid(currentWikiId, parents))
            return GetAlternativeWiki(pageCacheItem, sessionUser, parents);

        return EntityCache.GetPage(currentWikiId);
    }

    private bool IsCurrentWikiValid(int currentWikiId, IList<PageCacheItem> parents) =>
        parents.Any(c => c.Id == currentWikiId) && 
        currentWikiId > 0 && 
        _permissionCheck.CanView(EntityCache.GetPage(currentWikiId));


    private PageCacheItem GetAlternativeWiki(PageCacheItem pageCacheItem, SessionUser sessionUser, IList<PageCacheItem> parents)
    {
        var creatorWikiId = pageCacheItem.Creator.StartPageId;
        if (_permissionCheck.CanView(EntityCache.GetPage(creatorWikiId)))
        {
            var newWiki = parents.FirstOrDefault(c => c.Id == creatorWikiId) ?? GetUserWiki(sessionUser, parents);
            if (newWiki != null)
                return newWiki;
        }

        return parents.FirstOrDefault(p => p.IsWikiType()) ?? FeaturedPage.GetRootPage;
    }

    private PageCacheItem? GetUserWiki(SessionUser sessionUser, IList<PageCacheItem> parents)
    {
        if (!sessionUser.IsLoggedIn)
            return null;

        var userWikiId = _extendedUserCache.GetUser(sessionUser.UserId).StartPageId;
        var userWiki = EntityCache.GetPage(userWikiId);

        if (parents.Any(c => c.Id == userWiki?.Id))
            return userWiki;

        return null;
    }

    public int? SuggestNewParent(Crumbtrail breadcrumb, bool hasPublicQuestion)
    {
        CrumbtrailItem breadcrumbItem;

        if (hasPublicQuestion)
        {
            if (breadcrumb.Items.Any(i => i.Page.Visibility == PageVisibility.Public))
            {
                breadcrumbItem = breadcrumb.Items.Last(i => i.Page.Visibility == PageVisibility.Public);
                return breadcrumbItem.Page.Id;
            }

            return null;
        }

        var parent = breadcrumb.Items.LastOrDefault();
        if (parent == null)
            return _sessionUser.User.StartPageId;

        return parent.Page.Id;
    }
}