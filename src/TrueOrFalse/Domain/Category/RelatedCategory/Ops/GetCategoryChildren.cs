using System;
using System.Collections.Generic;
using System.Linq;

public class GetCategoryChildren
{
    public static List<CategoryCacheItem> WithAppliedRules(CategoryCacheItem category)
    {
        var categoriesToExclude = new List<CategoryCacheItem>();
        foreach (var categoryToExclude in category.CategoriesToExclude())
        {
            categoriesToExclude.Add(categoryToExclude);
            categoriesToExclude.AddRange(CategoryCacheItem.ToCacheCategories(EntityCache.GetDescendants(categoryToExclude.Id)).ToList());
        }

        var categoriesToInclude = new List<CategoryCacheItem>();
        foreach (var categoryToInclude in category.CategoriesToInclude())
        {
            categoriesToInclude.Add(categoryToInclude);
            categoriesToInclude.AddRange(CategoryCacheItem
                .ToCacheCategories(EntityCache.GetDescendants(categoryToInclude.Id)));
        }

        return CategoryCacheItem.ToCacheCategories(EntityCache.GetDescendants(category.Id)).Except(categoriesToExclude)
            .Union(categoriesToInclude)
            .ToList();

    }

    public static List<Category> WithAppliedRulesFromMemory(Category category)
    {
        throw new Exception("not done yet");

        //consider wishknowlede, visibilty and exclude and included categories
        return EntityCache.GetChildren(category);
    }
}