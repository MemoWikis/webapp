using System.Collections.Generic;
using System.Linq;

public class GetBreadCrumb
{
    public static IList<Category> For(Category category)
    {
        var result = GetParent(category, new List<Category>());
        result.Reverse();
        return result;
    }

    private static List<Category> GetParent(Category category, List<Category> result)
    {
        if (!category.ParentCategories().Any())
            return result;

        var defaultCategories = Sl.CategoryRepo.GetDefaultCategoriesList();
        var categoryToAdd = category.ParentCategories().First();
        foreach (var parentCategory in category.ParentCategories())
        {
            if (defaultCategories.Contains(parentCategory))
                categoryToAdd = parentCategory;
        }
        result.Add(categoryToAdd);

        return GetParent(categoryToAdd, result);
    }
}