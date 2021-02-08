using System;
using System.Collections.Generic;
using System.Linq;

public class GetCategoryChildren
{
    public static List<Category> WithAppliedRules(Category category)
    {
        var categoriesToExclude = new List<Category>();
        foreach (var categoryToExclude in category.CategoriesToExclude())
        {
            categoriesToExclude.Add(categoryToExclude);
            categoriesToExclude.AddRange(Sl.CategoryRepo.GetDescendants(categoryToExclude.Id));
        }

        var categoriesToInclude = new List<Category>();
        foreach (var categoryToInclude in category.CategoriesToInclude())
        {
            categoriesToInclude.Add(categoryToInclude);
            categoriesToInclude.AddRange(Sl.CategoryRepo.GetDescendants(categoryToInclude.Id));
        }

        return Sl.CategoryRepo.GetDescendants(category.Id)
            .Except(categoriesToExclude)
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