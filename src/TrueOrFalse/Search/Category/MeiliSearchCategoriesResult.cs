using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{//Todo: Mark to Delete
    public class MeiliSearchCategoriesResult : ISearchCategoriesResult
    {
        /// <summary>Amount of items found</summary>
        public int Count { get; set;}

        public List<int> CategoryIds { get; set; }

        public IPager Pager { get; set;}

        public IList<Category> GetCategories()  => Sl.CategoryRepo.GetByIds(CategoryIds); //todo: das ist doch nicht schön warum denn aus der Db statt ausm Cache.

        public MeiliSearchCategoriesResult()
        {
            CategoryIds = new List<int>(); 
        }
    }
}
