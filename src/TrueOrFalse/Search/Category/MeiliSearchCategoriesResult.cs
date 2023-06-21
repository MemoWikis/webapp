using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search
{//Todo: Mark to Delete
    public class MeiliSearchCategoriesResult : ISearchCategoriesResult
    {
        /// <summary>Amount of items found</summary>
        public int Count { get; set;}

        public List<int> CategoryIds { get; set; }

        public IPager Pager { get; set;}

        public IList<CategoryCacheItem> GetCategories() => EntityCache.GetCategories(CategoryIds).ToList();

        public MeiliSearchCategoriesResult()
        {
            CategoryIds = new List<int>(); 
        }
    }
}
