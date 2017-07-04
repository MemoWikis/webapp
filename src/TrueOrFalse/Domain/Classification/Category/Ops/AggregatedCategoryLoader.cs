using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class AggregatedCategoryLoader
{    
    public static IList<Category> FromDb(Category category, bool includeSelf = true)
    {
        var categoryRelations = Sl.CategoryRelationRepo.GetAll().ToConcurrentDictionary();

        return FilterCategories(category, categoryRelations, includeSelf);
    }

    private static IList<Category> FilterCategories(Category category, ConcurrentDictionary<int, CategoryRelation> categoryRelations, bool includeSelf)
    {
        var aggregatedCategories = categoryRelations
            .Where(r => r.Value.Category.Id == category.Id && r.Value.CategoryRelationType == CategoryRelationType.IncludesContentOf)
            .Select(r => r.Value.RelatedCategory);

        if (includeSelf)
            aggregatedCategories = aggregatedCategories.Union(new List<Category> {category});

        return aggregatedCategories.ToList();
    }

    public static IList<Category> FromCache(Category category, bool includeSelf = true)
    {
        //return FilterCategories(category, EntityCache.CategoryRelations, includeSelf);

        //var list =

        // category.CategoryRelations.Where(r => r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
        //    .Select(r => r.RelatedCategory).ToList();

        //return list;

        return null;
    }
}