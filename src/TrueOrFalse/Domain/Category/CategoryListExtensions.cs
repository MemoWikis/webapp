using System.Collections.Generic;
using System.Linq;

public static class CategoryListExtensions
{
    public static Category ByName(this IEnumerable<Category> categories, string name) => 
        categories.First(c => c.Name == name);

    public static CategoryCacheItem ByName(this IEnumerable<CategoryCacheItem> categories, string name) =>
        categories.First(c => c.Name == name);

    public static IEnumerable<int> GetIds(this IEnumerable<CategoryCacheItem> sets)
    {
        var l = sets.Select(q => q.Id).ToList();
        return l;
    }
        
}