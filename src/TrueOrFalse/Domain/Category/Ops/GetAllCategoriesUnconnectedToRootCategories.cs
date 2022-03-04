using System.Collections.Generic;
using System.Linq;

public class GetAllCategoriesUnconnectedToRootCategories
{
    public static List<CategoryCacheItem> Run()
    {
        var result = new List<CategoryCacheItem>();
        var categories = EntityCache.GetAllCategories();
        var rootCategories = new List<CategoryCacheItem>();
        rootCategories.Add(EntityCache.GetCategory(CategoryRepository.StudiumId));
        rootCategories.Add(EntityCache.GetCategory(CategoryRepository.SchuleId));
        rootCategories.Add(EntityCache.GetCategory(CategoryRepository.ZertifikateId));
        rootCategories.Add(EntityCache.GetCategory(CategoryRepository.AllgemeinwissenId));
        foreach (var category in categories)
        {
            var parentCategories = GraphService.GetAllParentsFromEntityCache(category.Id);
            if (!parentCategories.Intersect(rootCategories).Any())
            {
                result.Add(EntityCache.GetCategory(category.Id));
            }
        }
        return result;
    }
}
