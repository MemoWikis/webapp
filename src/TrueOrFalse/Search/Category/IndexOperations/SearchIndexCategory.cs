using System.Collections.Generic;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SearchIndexCategory : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<CategorySolrMap> _solrOperations;

        public SearchIndexCategory(ISolrOperations<CategorySolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(Category category)
        {
            _solrOperations.Add(ToCategorytSolrMap.Run(category));
            _solrOperations.Commit();
        }

        public void Update(IList<Category> categories)
        {
            foreach(var category in categories)
                _solrOperations.Add(ToCategorytSolrMap.Run(category));

            _solrOperations.Commit();
        }

        public void Delete(Category category)
        {
            _solrOperations.Delete(ToCategorytSolrMap.Run(category));
            _solrOperations.Commit();
        }
    }
}