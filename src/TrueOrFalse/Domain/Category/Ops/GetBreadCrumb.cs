using System.Collections.Generic;
using System.Linq;

public class GetBreadCrumb
{
    public static IList<CategoryCacheItem> For(CategoryCacheItem categoryCacheItem)
    {
        if(categoryCacheItem == null)
            return new List<CategoryCacheItem>();

        var result = GetParent(categoryCacheItem, new List<CategoryCacheItem>());
        result.Reverse();
        return result;
    }

    private static List<CategoryCacheItem> GetParent(CategoryCacheItem categoryCacheItem, List<CategoryCacheItem> result)
    {
        var defaultCategories = EntityCache.GetCategoryCacheItems( Sl.CategoryRepo.GetRootCategoriesListÍds());
        if (!categoryCacheItem.ParentCategories().Any() || defaultCategories.Contains(categoryCacheItem))
            return result;

        var categoryToAdd = categoryCacheItem.ParentCategories().First();
        foreach (var parentCategory in categoryCacheItem.ParentCategories())
        {
            if (defaultCategories.Contains(parentCategory))
                categoryToAdd = parentCategory;
        }
        result.Add(categoryToAdd);

        return GetParent(categoryToAdd, result);
    }
}