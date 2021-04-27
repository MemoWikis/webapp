using System.Collections.Generic;
using System.Linq;

public class GetAllCategoriesInSeveralRootCategories
{
    public static List<CategoryCacheItem> Run()
    {
        var result = new List<CategoryCacheItem>();
        var categories = EntityCache.GetAllCategories();
        var rootCategories = EntityCache.GetCategoryCacheItems(Sl.CategoryRepo.GetRootCategoryInts());
        foreach (var category in categories)
        {
            var parentCategories = GraphService.GetAllParentsFromEntityCache(category.Id);
            if (parentCategories.Intersect(rootCategories).Any())
            {
                result.Add(category);
            }
        }
        return result;
    }
}
