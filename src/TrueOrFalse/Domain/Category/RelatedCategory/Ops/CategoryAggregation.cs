using System.Collections.Generic;
using System.Linq;

class CategoryAggregation
{
    public static IList<Category> GetAggregatingAncestors(IList<Category> categories, CategoryRepository categoryRepository)
    {
        return categories.SelectMany(c => categoryRepository.GetIncludingCategories(c)).Distinct().ToList();
    }
}
