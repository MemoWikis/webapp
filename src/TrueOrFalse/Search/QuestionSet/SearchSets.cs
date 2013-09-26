using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SearchSets : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<SetSolrMap> _searchOperations;

        public SearchSets(ISolrOperations<SetSolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchSetsResult Run(string searchTearm, int creatorId = -1){
            return Run(searchTearm, new Pager(), creatorId);
        }

        public SearchSetsResult Run(string searchTearm, Pager pager, int creatorId = -1)
        {
            var sqb = new SearchQueryBuilder()
                .Add("FullTextStemmed", searchTearm)
                .Add("FullTextExact", searchTearm)
                .Add("CreatorId", 
                    creatorId != -1 ? creatorId.ToString() : null, 
                    isMustHave: true,
                    exact: true);

            var queryResult = _searchOperations.Query(sqb.ToString(),
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true}
                                                      });

            var result = new SearchSetsResult();
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