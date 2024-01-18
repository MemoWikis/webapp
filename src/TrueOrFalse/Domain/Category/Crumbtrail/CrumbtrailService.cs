public class CrumbtrailService : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUserCache _sessionUserCache;

    public CrumbtrailService(
        PermissionCheck permissionCheck,
        SessionUserCache sessionUserCache)
    {
        _permissionCheck = permissionCheck;
        _sessionUserCache = sessionUserCache;
    }
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
                foreach (var parent in parents)
                {
                    if (result.Rootfound)
                        break;
                    if (IsLinkedToRoot(parent, root))
                        AddBreadcrumbParent(result, parent, root);
                }

            BreadCrumbtrailCorrectnessCheck(result.Items, category);
        }

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private void BreadCrumbtrailCorrectnessCheck(IList<CrumbtrailItem> crumbtrailItems, CategoryCacheItem category)
    {
        if (crumbtrailItems == null || crumbtrailItems.Count == 0)
            return;

        if (category.Parents().All(c => c.Id != crumbtrailItems[0].Category.Id))
            Logg.r.Error("Breadcrumb - {currentCategoryId}: next item is not a direct parent, currentItemId: {categoryId}, nextItemId: {nextItemId}", category.Id, category.Id, crumbtrailItems[0].Category.Id);
        
        for (int i = 0; i < crumbtrailItems.Count - 1; i++)
        {
            var categoryCacheItem = crumbtrailItems[i].Category;
            var nextItemId = crumbtrailItems[i + 1].Category.Id;

            if (!_permissionCheck.CanView(categoryCacheItem))
                Logg.r.Error("Breadcrumb - {currentCategoryId}: visibility/permission", category.Id);

            if (categoryCacheItem.Parents().All(c => c.Id != nextItemId))
                Logg.r.Error("Breadcrumb - {currentCategoryId}: next item is not a direct parent, currentItemId: {categoryId}, nextItemId: {nextItemId}", category.Id, categoryCacheItem.Id, nextItemId);
        }
    }

    private bool IsLinkedToRoot(CategoryCacheItem category, CategoryCacheItem root)
    {
        var isLinkedToRoot = GraphService.VisibleAscendants(category.Id,_permissionCheck).Any(c => c == root);
        if (isLinkedToRoot)
            return true;
        return false;
    }

    private void AddBreadcrumbParent(Crumbtrail crumbtrail, CategoryCacheItem categoryCacheItem, CategoryCacheItem root)
    {
        if (_permissionCheck.CanView(categoryCacheItem))
            crumbtrail.Add(categoryCacheItem);
        else return;

        if (root == categoryCacheItem)
            return;

        var parents = GraphService.VisibleParents(categoryCacheItem.Id,_permissionCheck);
        parents = OrderParentList(parents, root.Creator.Id);
        
        if (parents.Any(c => c.Id == root.Id))
            crumbtrail.Add(root);
        else
            foreach (var parent in parents)
            {
                if (crumbtrail.Rootfound)
                    break;

                if (parent == root)
                {
                    if (_permissionCheck.CanView(parent))
                        crumbtrail.Add(parent);
                    break;
                }

                if (IsLinkedToRoot(parent, root))
                    AddBreadcrumbParent(crumbtrail, parent, root);
            }

    }

    private static List<CategoryCacheItem> OrderParentList(IList<CategoryCacheItem> parents, int wikiCreatorId)
    {
        var parentsWithWikiCreator = parents.Where(c => c.Creator.Id == wikiCreatorId).Select(c => c);
        var parentsWithoutWikiCreator = parents.Where(c => c.Creator.Id != wikiCreatorId).Select(c => c);
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
        if (parents.All(c => c.Id != currentWikiId) || currentWikiId <= 0 || !_permissionCheck.CanView(EntityCache.GetCategory(currentWikiId)))
        {
            if (categoryCacheItem.Creator != null)
            {
                var creatorWikiId = categoryCacheItem.Creator.StartTopicId;
                if (_permissionCheck.CanView(EntityCache.GetCategory(creatorWikiId)))
                {
                    if (parents.Any(c => c.Id == creatorWikiId))
                    {
                        var newWiki = parents.FirstOrDefault(c => c.Id == creatorWikiId);
                        return newWiki;
                    }

                    if (sessionUser.IsLoggedIn)
                    {
                        var userWikiId = _sessionUserCache.GetUser(sessionUser.UserId).StartTopicId;
                        var userWiki = EntityCache.GetCategory(userWikiId);
                        if (parents.Any(c => c == userWiki))
                            return userWiki;
                    }
                }
            }

            return RootCategory.Get;
        }

        return EntityCache.GetCategory(currentWikiId);
    }
}
