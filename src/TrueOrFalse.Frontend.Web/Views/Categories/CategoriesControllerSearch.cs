using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Search;

public class CategoriesControllerSearch
{
    public IList<CategoryCacheItem> Run() => Run(Sl.R<SessionUiData>().SearchSpecCategory);

    public IList<CategoryCacheItem> Run(CategorySearchSpec searchSpec)
    {
        var solrResult = Sl.R<SearchCategories>().Run(searchSpec);
        return EntityCache
            .CategoryCacheItemsForSearch(solrResult.CategoryIds.ToArray());
    }
}