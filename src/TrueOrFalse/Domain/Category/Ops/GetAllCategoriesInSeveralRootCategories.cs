using System.Collections.Generic;
using System.Linq;

public class GetAllCategoriesInSeveralRootCategories
{
    public static List<Category> Run()
    {
        var result = new List<Category>();
        var categories = EntityCache.GetAllCategories();
        var rootCategories = EntityCache.GetCategoryCacheItems(Sl.CategoryRepo.GetRootCategoryInts());
        foreach (var category in categories)
        {
            var parentCategories = GraphService.GetAllParents(category);
            if (parentCategories.Intersect(rootCategories).Any())
            {
                result.Add(category);
            }
        }
        return result;
    }
}
