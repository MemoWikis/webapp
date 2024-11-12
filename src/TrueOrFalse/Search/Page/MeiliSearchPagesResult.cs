using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search
{
    public class MeiliSearchPagesResult : ISearchPagesResult
    {
        /// <summary>Amount of items found</summary>
        public int Count { get; set; }

        public List<int> PageIds { get; set; }

        public IPager Pager { get; set; }

        public IList<PageCacheItem> GetPages() =>
            EntityCache.GetPages(PageIds).ToList();

        public MeiliSearchPagesResult()
        {
            PageIds = new List<int>();
        }
    }
}