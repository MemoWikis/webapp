using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Delete(Category category)
        {
            _solrOperations.Delete(ToCategorytSolrMap.Run(category));
            _solrOperations.Commit();
        }
    }
}