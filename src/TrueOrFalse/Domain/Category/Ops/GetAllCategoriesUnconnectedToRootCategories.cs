using System.Collections.Generic;
using System.Linq;

public class GetAllCategoriesUnconnectedToRootCategories
{
    public static List<Category> Run()
    {
        var result = new List<Category>();
        var categories = EntityCache.GetAllCategories();
        var rootCategories = Sl.R<CategoryRepository>().GetRootCategoriesList();
        foreach (var category in categories)
        {
            var parentCategories = GraphService.GetAllParents(category.Id);
            if (!parentCategories.Intersect(rootCategories).Any())
            {
                result.Add(category);
            }
        }
        return result;
    }
}
