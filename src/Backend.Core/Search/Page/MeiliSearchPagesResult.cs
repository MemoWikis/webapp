public class MeilisearchPagesResult : ISearchPagesResult
{
    /// <summary>Amount of items found</summary>
    public int Count { get; set; }

    public List<int> PageIds { get; set; } = new();

    public IPager Pager { get; set; } = new Pager();

    public List<PageCacheItem> GetPages() => EntityCache.GetPages(PageIds).ToList();
}