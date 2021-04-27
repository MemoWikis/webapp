using System.Collections.Generic;
using System.Linq;
using Iesi.Collections.Generic;

public class GetAllCategoriesUnconnectedToRootCategories
{
    public static List<CategoryCacheItem> Run()
    {
        var result = new List<CategoryCacheItem>();
        var categories = EntityCache.GetAllCategories();
        var rootCategories = new List<CategoryCacheItem>();
        rootCategories.Add(EntityCache.GetCategoryCacheItem(CategoryRepository.StudiumId));
        rootCategories.Add(EntityCache.GetCategoryCacheItem(CategoryRepository.SchuleId));
        rootCategories.Add(EntityCache.GetCategoryCacheItem(CategoryRepository.ZertifikateId));
        rootCategories.Add(EntityCache.GetCategoryCacheItem(CategoryRepository.AllgemeinwissenId));
        foreach (var category in categories)
        {
            var parentCategories = GraphService.GetAllParentsFromEntityCache(category.Id);
            if (!parentCategories.Intersect(rootCategories).Any())
            {
                result.Add(EntityCache.GetCategoryCacheItem(category.Id));
            }
        }
        return result;
    }
}
