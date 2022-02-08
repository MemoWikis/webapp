using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;

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
            parents = OrderParentList(parents, root.Creator);
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
        }

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private static bool IsLinkedToRoot(CategoryCacheItem category, CategoryCacheItem root)
    {
        var isLinkedToRoot = EntityCache.GetAllParents(category.Id).Any(c => c == root);
        if (isLinkedToRoot)
            return true;
        return false;
    }

    private static void AddBreadcrumbParent(Crumbtrail crumbtrail, CategoryCacheItem categoryCacheItem, CategoryCacheItem root)
    {
        if (PermissionCheck.CanView(categoryCacheItem))
            crumbtrail.Add(categoryCacheItem);

        if (root == categoryCacheItem)
            return;

        var parents = categoryCacheItem.ParentCategories();
        parents = OrderParentList(parents, root.Creator);
        
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

    private static List<CategoryCacheItem> OrderParentList(IList<CategoryCacheItem> parents, User wikiCreator)
    {
        var parentsWithWikiCreator = parents.Where(c => c.Creator == wikiCreator).Select(c => c);
        var parentsWithoutWikiCreator = parents.Where(c => c.Creator != wikiCreator).Select(c => c);
        var orderedParents = new List<CategoryCacheItem>();
        orderedParents.AddRange(parentsWithWikiCreator);
        orderedParents.AddRange(parentsWithoutWikiCreator);

        return orderedParents;
    }

    public static CategoryCacheItem GetWiki(CategoryCacheItem categoryCacheItem)
    {
        var sessionUser = Sl.SessionUser;
        var currentWikiId = sessionUser.CurrentWikiId;
        if (categoryCacheItem.IsStartPage())
            return categoryCacheItem;

        var parents = EntityCache.GetAllParents(categoryCacheItem.Id, true);
        if (parents.All(c => c.Id != currentWikiId) || currentWikiId <= 0 || !PermissionCheck.CanView(EntityCache.GetCategoryCacheItem(currentWikiId)))
        {
            if (categoryCacheItem.Creator != null)
            {
                var creatorWikiId = categoryCacheItem.Creator.StartTopicId;
                if (PermissionCheck.CanView(EntityCache.GetCategoryCacheItem(creatorWikiId)))
                {
                    if (parents.Any(c => c.Id == creatorWikiId))
                    {
                        var newWiki = parents.FirstOrDefault(c => c.Id == creatorWikiId);
                        return newWiki;
                    }

                    if (sessionUser.IsLoggedIn)
                    {
                        var userWikiId = UserCache.GetUser(sessionUser.UserId).StartTopicId;
                        var userWiki = EntityCache.GetCategoryCacheItem(userWikiId);
                        if (parents.Any(c => c == userWiki))
                            return userWiki;
                    }
                }
            }

            return RootCategory.Get;
        }

        return EntityCache.GetCategoryCacheItem(currentWikiId);
    }
}
