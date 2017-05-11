using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{
    public class SearchCategoriesResult
    {
        /// <summary>In milliseconds</summary>
        public int QueryTime;

        /// <summary>Amount of items found</summary>
        public int Count;

        public SpellCheckResults SpellChecking;

        public List<int> CategoryIds = new List<int>();

        public IPager Pager;

        public IList<Category> GetCategories() => Sl.CategoryRepo.GetByIds(CategoryIds);
    }
}
