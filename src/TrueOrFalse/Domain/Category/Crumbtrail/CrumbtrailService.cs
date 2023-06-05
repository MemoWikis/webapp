using System.Collections.Generic;
using System.Linq;

public class CrumbtrailService
{
    public static Crumbtrail Get(CategoryCacheItem category, CategoryCacheItem root)
    {
        var result = new Crumbtrail(category, root);
        if (!category.IsStartPage())
        {
            var parents = category.ParentCategories();
            var rootWikiParent = parents.FirstOrDefault(c => c == root);
            if (rootWikiParent != null)
                AddParent(result, rootWikiParent, root);
            else
                foreach (var parent in parents)
                    AddParent(result, parent, root);
        }

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private static void AddParent(Crumbtrail crumbtrail, CategoryCacheItem categoryCacheItem, CategoryCacheItem root)
    {
        if (crumbtrail.Rootfound)
            return;

        crumbtrail.Add(categoryCacheItem);
        var parents = categoryCacheItem.ParentCategories();
        if (parents.Any(c => c == root))
            AddParent(crumbtrail, root, root);
        else
            foreach (var currentCategory in parents)
            {
                if (crumbtrail.AlreadyAdded(currentCategory))
                    return;

                AddParent(crumbtrail, currentCategory, root);
            }
    }

    public static Crumbtrail BuildCrumbtrail(CategoryCacheItem category, CategoryCacheItem root)
    {
        var result = new Crumbtrail(category, root);
        if (!category.IsStartPage())
        {
            var parents = category.ParentCategories();
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

    private static void BreadCrumbtrailCorrectnessCheck(IList<CrumbtrailItem> crumbtrailItems, CategoryCacheItem category)
    {
        if (crumbtrailItems == null || crumbtrailItems.Count == 0)
            return;

        if (category.ParentCategories().All(c => c.Id != crumbtrailItems[0].Category.Id))
            Logg.r().Error("Breadcrumb - {currentCategoryId}: next item is not a direct parent, currentItemId: {categoryId}, nextItemId: {nextItemId}", category.Id, category.Id, crumbtrailItems[0].Category.Id);
        
        for (int i = 0; i < crumbtrailItems.Count - 1; i++)
        {
            var categoryCacheItem = crumbtrailItems[i].Category;
            var nextItemId = crumbtrailItems[i + 1].Category.Id;

            if (!PermissionCheck.CanView(categoryCacheItem))
                Logg.r().Error("Breadcrumb - {currentCategoryId}: visibility/permission", category.Id);

            if (categoryCacheItem.ParentCategories().All(c => c.Id != nextItemId))
                Logg.r().Error("Breadcrumb - {currentCategoryId}: next item is not a direct parent, currentItemId: {categoryId}, nextItemId: {nextItemId}", category.Id, categoryCacheItem.Id, nextItemId);
        }
    }

    private static bool IsLinkedToRoot(CategoryCacheItem category, CategoryCacheItem root)
    {
        var isLinkedToRoot = EntityCache.GetAllParents(category.Id, visibleOnly:true).Any(c => c == root);
        if (isLinkedToRoot)
            return true;
        return false;
    }

    private static void AddBreadcrumbParent(Crumbtrail crumbtrail, CategoryCacheItem categoryCacheItem, CategoryCacheItem root)
    {
        if (PermissionCheck.CanView(categoryCacheItem))
            crumbtrail.Add(categoryCacheItem);
        else return;

        if (root == categoryCacheItem)
            return;

        var parents = EntityCache.ParentCategories(categoryCacheItem.Id, visibleOnly:true);
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
                    if (PermissionCheck.CanView(parent))
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

    public static CategoryCacheItem GetWiki(CategoryCacheItem categoryCacheItem)
    {
        var currentWikiId = SessionUserLegacy.CurrentWikiId;
        if (categoryCacheItem.IsStartPage())
            return categoryCacheItem;

        var parents = EntityCache.GetAllParents(categoryCacheItem.Id, true, true);
        if (parents.All(c => c.Id != currentWikiId) || currentWikiId <= 0 || !PermissionCheck.CanView(EntityCache.GetCategory(currentWikiId)))
        {
            if (categoryCacheItem.Creator != null)
            {
                var creatorWikiId = categoryCacheItem.Creator.StartTopicId;
                if (PermissionCheck.CanView(EntityCache.GetCategory(creatorWikiId)))
                {
                    if (parents.Any(c => c.Id == creatorWikiId))
                    {
                        var newWiki = parents.FirstOrDefault(c => c.Id == creatorWikiId);
                        return newWiki;
                    }

                    if (SessionUserLegacy.IsLoggedIn)
                    {
                        var userWikiId = SessionUserCache.GetUser(SessionUserLegacy.UserId).StartTopicId;
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
