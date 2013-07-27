using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SearchSet : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<SetSolrMap> _searchOperations;

        public SearchSet(ISolrOperations<SetSolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchSetResult Run(string searchTearm){
            return Run(searchTearm, new Pager());
        }

        public SearchSetResult Run(string searchTearm, Pager pager)
        {
            var queryResult = _searchOperations.Query("FullTextStemmed:" + searchTearm.Trim() + " " +
                                                      "FullTextExact:" + searchTearm.Trim() +
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true},
                                                            
                                                      });

            var result = new SearchSetResult();
            result.QueryTime = queryResult.Header.QTime;
            result.Count = queryResult.NumFound;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;

            pager.TotalItems = result.Count;

            foreach (var resultItem in queryResult)
                result.SetIds.Add(resultItem.Id);

            return result;
        }
    }
}