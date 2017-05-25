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

        var categoryToAdd = category.ParentCategories().First();
        result.Add(categoryToAdd);

        return GetParent(categoryToAdd, result);
    }
}