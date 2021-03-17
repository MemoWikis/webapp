using System.Linq;

public class CrumbtrailService
{
    public static Crumbtrail Get(CategoryCacheItem category, CategoryCacheItem root)
    {
        var result = new Crumbtrail(category, root);
        var parents = category.ParentCategories();
        foreach (var parent in parents) 
            AddParent(result, parent);

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private static void AddParent(Crumbtrail crumbtrail, CategoryCacheItem categoryCacheItem)
    {
        if (crumbtrail.Rootfound)
            return;

        crumbtrail.Add(categoryCacheItem);

        foreach (var currentCategory in categoryCacheItem.ParentCategories())
        {
            if (crumbtrail.AlreadyAdded(currentCategory))
                return;

            AddParent(crumbtrail, currentCategory);
        }
    }
}
