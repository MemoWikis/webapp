using System.Collections.Generic;
using System.Linq;

public static class CategoryListExtensions
{
    public static Category ByName(this IEnumerable<Category> categories, string name) => 
        categories.First(c => c.Name == name);

    public static string GetValueByIndex(this IEnumerable<Category> categories, int index)
    {
        if (categories != null && categories.Count() > index)
            return categories.ToList()[index].Name;

        return "";
    }

    public static IEnumerable<int> GetIds(this IEnumerable<Category> sets) =>
        sets.Select(q => q.Id).ToList();
}