using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SaveCategoryToIndex : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<CategorySolrMap> _solrOperations;

        public SaveCategoryToIndex(ISolrOperations<CategorySolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Run(Category category)
        {
            _solrOperations.Add(ToCategorytSolrMap.Run(category));
            _solrOperations.Commit();
        }
    }
}