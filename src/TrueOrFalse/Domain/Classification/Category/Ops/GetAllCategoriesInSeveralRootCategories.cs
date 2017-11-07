using System.Collections.Generic;
using System.Linq;

public class GetAllCategoriesInSeveralRootCategories
{
    public static List<Category> Run()
    {
        var result = new List<Category>();
        var categories = EntityCache.GetAllCategories();
        var rootCategories = Sl.R<CategoryRepository>().GetRootCategoriesList();
        foreach (var category in categories)
        {
            var parentCategories = Sl.R<CategoryRepository>().GetAllParents(category.Id);
            if (parentCategories.Intersect(rootCategories).Count()>1)
            {
                result.Add(category);
            }
        }
        return result;
    }
}
