using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search
{
    public class MeiliSearchCategoriesResult : ISearchCategoriesResult
    {
        /// <summary>Amount of items found</summary>
        public int Count { get; set; }

        public List<int> CategoryIds { get; set; }

        public IPager Pager { get; set; }

        public IList<PageCacheItem> GetCategories() =>
            EntityCache.GetCategories(CategoryIds).ToList();

        public MeiliSearchCategoriesResult()
        {
            CategoryIds = new List<int>();
        }
    }
}