using System.Collections.Generic;
using System.Linq;

public static class CategoryValuationExt
{
    public static IList<int> GetCategoryIds(this IEnumerable<CategoryValuation> valuations)
    {
        return valuations.Select(valuation => valuation.CategoryId).ToList();
    }
}