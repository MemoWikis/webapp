using System.Collections.Generic;
using System.Linq;

class CategoryAggregation
{
    public static IList<Category> GetInterrelatedCategories(IList<Category> categories)
    {
        return categories.SelectMany(c => Sl.CategoryRepo.GetIncludingCategories(c)).Distinct().ToList();
    }
}
