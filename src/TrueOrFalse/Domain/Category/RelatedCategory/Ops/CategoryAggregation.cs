using System.Collections.Generic;
using System.Linq;

class CategoryAggregation
{
    public static IList<Category> GetAggregatingAncestors(IList<Category> categories)
    {
        return categories.SelectMany(c => Sl.CategoryRepo.GetIncludingCategories(c)).Distinct().ToList();
    }
}
