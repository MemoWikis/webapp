using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class RemoveCategoryFromIndex : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<CategorySolrMap> _solrOperations;

        public RemoveCategoryFromIndex(ISolrOperations<CategorySolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Run(Category category)
        {
            _solrOperations.Delete(ToCategorytSolrMap.Run(category));
            _solrOperations.Commit();
        }
    }
}
