using System;
using System.Collections.Generic;
using System.Linq;

public class AggregatedCategoryLoader
{    
    public static IList<Category> FromDb(Category category, bool includeSelf = false)
    {
        var categoryRelations = Sl.CategoryRelationRepo.GetAll();

        return FilterCategories(category, categoryRelations, includeSelf);
    }

    private static IList<Category> FilterCategories(Category category, IList<CategoryRelation> categoryRelations, bool includeSelf)
    {
        var aggregatedCategories = categoryRelations
            .Where(r => r.Category == category && r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
            .Select(r => r.RelatedCategory);

        if (includeSelf)
            aggregatedCategories = aggregatedCategories.Union(new List<Category> {category});

        return aggregatedCategories.ToList();
    }


    public static IList<Category> FromMemory(Category category)
    {
        return FilterCategories(category, CategoryCache.CategoryRelations, true);

    }


}