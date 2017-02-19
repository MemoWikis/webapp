using System.Collections.Generic;
using System.Linq;

public static class CategoryValuationExt
{
    public static IList<int> GetCategoryIds(this IEnumerable<CategoryValuation> valuations) => 
        valuations.Select(valuation => valuation.CategoryId).ToList();

    public static CategoryValuation ByCategoryId(this IEnumerable<CategoryValuation> valuations, int categoryId) =>
        valuations.FirstOrDefault(x => x.CategoryId == categoryId);
}