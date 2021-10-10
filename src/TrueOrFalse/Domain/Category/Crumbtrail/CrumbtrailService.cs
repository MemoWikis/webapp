using System.Linq;
using FluentNHibernate.Conventions.Inspections;

public class CrumbtrailService
{
    public static Crumbtrail Get(CategoryCacheItem category, CategoryCacheItem root)
    {
        var result = new Crumbtrail(category, root);
        if (!category.IsRootWiki())
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
}
