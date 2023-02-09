using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{//Todo: Mark to Delete
    public class SolrSearchCategoriesResult : ISearchCategoriesResult
    {
        public int QueryTime;
        public int Count { get; set; }
        public SpellCheckResults SpellChecking;
        public List<int> CategoryIds { get; set;  } = new List<int>();
        public IPager Pager { get; set; }
        public IList<CategoryCacheItem> GetCategories() => EntityCache.GetCategories(CategoryIds).ToList();
    }
}
