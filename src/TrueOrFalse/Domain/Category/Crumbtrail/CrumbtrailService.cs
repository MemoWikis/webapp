public class CrumbtrailService(
    PermissionCheck _permissionCheck,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public Crumbtrail BuildCrumbtrail(CategoryCacheItem category, CategoryCacheItem root)
    {
        var result = new Crumbtrail(category, root);

        if (!category.IsStartPage())
        {
            var parents = category.Parents();
            var rootWikiParent = parents.FirstOrDefault(c => c == root);
            parents = OrderParentList(parents, root.Creator.Id);

            if (rootWikiParent != null)
                AddBreadcrumbParent(result, rootWikiParent, root);
            else
                AddBreadcrumbParents(result, parents, root);

            CheckBreadcrumbTrailCorrectness(result.Items, category);
        }

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private void CheckBreadcrumbTrailCorrectness(IList<CrumbtrailItem> crumbtrailItems, CategoryCacheItem category)
    {
        if (crumbtrailItems.Count == 0)
            return;

        if (category.Parents().All(c => c.Id != crumbtrailItems[0].Category.Id))
            Logg.r.Error(
                "Breadcrumb - {currentCategoryId}: next item is not a direct parent, currentItemId: {categoryId}, nextItemId: {nextItemId}",
                category.Id, category.Id, crumbtrailItems[0].Category.Id);

        for (int i = 0; i < crumbtrailItems.Count - 1; i++)
        {
            var categoryCacheItem = crumbtrailItems[i].Category;
            var nextItemId = crumbtrailItems[i + 1].Category.Id;

            if (!_permissionCheck.CanView(categoryCacheItem))
                Logg.r.Error("Breadcrumb - {currentCategoryId}: visibility/permission", category.Id);

            if (categoryCacheItem.Parents().All(c => c.Id != nextItemId))
                Logg.r.Error(
                    "Breadcrumb - {currentCategoryId}: next item is not a direct parent, currentItemId: {categoryId}, nextItemId: {nextItemId}",
                    category.Id, categoryCacheItem.Id, nextItemId);
        }
    }

    private void AddBreadcrumbParents(Crumbtrail crumbtrail, IList<CategoryCacheItem> parents, CategoryCacheItem root)
    {
        foreach (var parent in parents)
        {
            if (crumbtrail.Rootfound)
                break;

            if (IsLinkedToRoot(parent, root))
                AddBreadcrumbParent(crumbtrail, parent, root);
        }
    }

    private bool IsLinkedToRoot(CategoryCacheItem category, CategoryCacheItem root)
    {
        return GraphService.VisibleAscendants(category.Id, _permissionCheck).Any(c => c == root);
    }

    private void AddBreadcrumbParent(Crumbtrail crumbtrail, CategoryCacheItem categoryCacheItem, CategoryCacheItem root)
    {
        if (_permissionCheck.CanView(categoryCacheItem))
            crumbtrail.Add(categoryCacheItem);
        else
            return;

        if (root == categoryCacheItem)
            return;

        var parents = GraphService.VisibleParents(categoryCacheItem.Id, _permissionCheck);
        parents = OrderParentList(parents, root.Creator.Id);

        if (parents.Any(c => c.Id == root.Id))
            crumbtrail.Add(root);
        else
            AddBreadcrumbParents(crumbtrail, parents, root);
    }

    private static List<CategoryCacheItem> OrderParentList(IList<CategoryCacheItem> parents, int wikiCreatorId)
    {
        var parentsWithWikiCreator = parents.Where(c => c.Creator.Id == wikiCreatorId).ToList();
        var parentsWithoutWikiCreator = parents.Where(c => c.Creator.Id != wikiCreatorId).ToList();

        var orderedParents = new List<CategoryCacheItem>();
        orderedParents.AddRange(parentsWithWikiCreator);
        orderedParents.AddRange(parentsWithoutWikiCreator);

        return orderedParents;
    }

    public CategoryCacheItem GetWiki(CategoryCacheItem categoryCacheItem, SessionUser sessionUser)
    {
        var currentWikiId = sessionUser.CurrentWikiId;

        if (categoryCacheItem.IsStartPage())
            return categoryCacheItem;

        var parents = GraphService.VisibleAscendants(categoryCacheItem.Id, _permissionCheck);
        if (!IsCurrentWikiValid(currentWikiId, parents))
            return GetAlternativeWiki(categoryCacheItem, sessionUser, parents);

        return EntityCache.GetCategory(currentWikiId);
    }

    private bool IsCurrentWikiValid(int currentWikiId, IList<CategoryCacheItem> parents) =>
        parents.Any(c => c.Id == currentWikiId)
        && currentWikiId > 0 
        && _permissionCheck.CanView(EntityCache.GetCategory(currentWikiId));


    private CategoryCacheItem GetAlternativeWiki(CategoryCacheItem categoryCacheItem, SessionUser sessionUser, IList<CategoryCacheItem> parents)
    {
        var creatorWikiId = categoryCacheItem.Creator.StartTopicId;
        if (_permissionCheck.CanView(EntityCache.GetCategory(creatorWikiId)))
        {
            var newWiki = parents.FirstOrDefault(c => c.Id == creatorWikiId) ?? GetUserWiki(sessionUser, parents);
            if (newWiki != null)
                return newWiki;
        }

        return parents.FirstOrDefault(p => p.IsStartPage()) ?? RootCategory.Get;
    }

    private CategoryCacheItem? GetUserWiki(SessionUser sessionUser, IList<CategoryCacheItem> parents)
    {
        if (!sessionUser.IsLoggedIn)
            return null;

        var userWikiId = _extendedUserCache.GetUser(sessionUser.UserId).StartTopicId;
        var userWiki = EntityCache.GetCategory(userWikiId);

        if (parents.Any(c => c.Id == userWiki?.Id))
            return userWiki;

        return null;
    }

}