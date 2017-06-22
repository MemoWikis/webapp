using System.Collections.Generic;
using System.Linq;

public class AggregatedCategoryLoader
{    
    public static IList<Category> FromDb(Category category, bool includeSelf = false)
    {
        var aggregatedCategories = Sl.CategoryRelationRepo
            .GetAll()
            .Where(r => r.Category == category && r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
            .Select(r => r.RelatedCategory);

        if(includeSelf)
            aggregatedCategories = aggregatedCategories.Union(new List<Category>{category});

        return aggregatedCategories.ToList();
    }
}