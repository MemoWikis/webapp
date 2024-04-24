using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search;

public interface ISearchCategoriesResult
{
    int Count { get; set; }
    List<int> CategoryIds { get; set; }
    IPager Pager { get; set; }

    IList<CategoryCacheItem> GetCategories();
}