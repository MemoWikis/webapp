using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SearchCategories : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<CategorySolrMap> _searchOperations;

        public SearchCategories(ISolrOperations<CategorySolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchCategoriesResult Run(string searchTearm){
            return Run(searchTearm, new Pager());
        }

        public SearchCategoriesResult Run(string searchTearm, Pager pager)
        {
            var sqb = new SearchQueryBuilder()
                .Add("FullTextStemmed", searchTearm)
                .Add("FullTextExact", searchTearm);

            var queryResult = _searchOperations.Query(sqb.ToString(),                            
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true}
                                                      });

            var result = new SearchCategoriesResult();
            result.QueryTime = queryResult.Header.QTime;
            result.Count = queryResult.NumFound;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;

            pager.TotalItems = result.Count;

            foreach (var resultItem in queryResult)
                result.CategoryIds.Add(resultItem.Id);

            return result;
        }
    }
}