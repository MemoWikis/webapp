using System.Linq;
using FluentNHibernate.Conventions.Inspections;

public class CrumbtrailService
{
    public static Crumbtrail Get(CategoryCacheItem category, CategoryCacheItem root)
    {
        var result = new Crumbtrail(category, root);
        if (!category.IsWiki())
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
        if (!category.IsWiki())
        {
            var parents = category.ParentCategories();
            var rootWikiParent = parents.FirstOrDefault(c => c == root);
            if (rootWikiParent != null)
                AddBreadcrumbParent(result, rootWikiParent, root);
            else
            {
                foreach (var parent in parents)
                {
                    if (result.Rootfound)
                        break;
                    if (IsLinkedToRoot(parent, root))
                        AddBreadcrumbParent(result, parent, root);
                }
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
        crumbtrail.Add(categoryCacheItem);
        if (root == categoryCacheItem)
            return;

        var parents = categoryCacheItem.ParentCategories();

        foreach (var parent in parents)
        {
            if (parent == root)
            {
                crumbtrail.Add(parent);
                break;
            }

            if (IsLinkedToRoot(parent, root))
                AddBreadcrumbParent(crumbtrail, parent, root);
        }

    }
}
