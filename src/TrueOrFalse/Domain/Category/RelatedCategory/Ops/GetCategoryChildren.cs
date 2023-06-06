using System.Collections.Generic;
using System.Linq;

public class GetCategoryChildren
{
    public static List<CategoryCacheItem> WithAppliedRules(CategoryCacheItem category, bool getFromEntityCache = true)
    {
        var categoriesToExclude = new List<CategoryCacheItem>();
        foreach (var categoryToExclude in category.CategoriesToExclude())
        {
            categoriesToExclude.Add(categoryToExclude);
            categoriesToExclude.AddRange(EntityCache.GetAllChildren(categoryToExclude.Id));
        }

        var categoriesToInclude = new List<CategoryCacheItem>();
        foreach (var categoryToInclude in category.CategoriesToInclude())
        {
            categoriesToInclude.Add(categoryToInclude);
            categoriesToInclude.AddRange(EntityCache.GetAllChildren(categoryToInclude.Id));
        }

        return EntityCache.GetAllChildren(category.Id, getFromEntityCache).Except(categoriesToExclude)
            .Union(categoriesToInclude)
            .ToList();
    }
}