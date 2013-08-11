using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SearchUsers : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<UserSolrMap> _searchOperations;

        public SearchUsers(ISolrOperations<UserSolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchUsersResult Run(string searchTearm){
            return Run(searchTearm, new Pager());
        }

        public SearchUsersResult Run(string searchTearm, Pager pager)
        {
            var searchExpression =
                "Name:" + InputToSearchExpression.Run(searchTearm);

            var queryResult = _searchOperations.Query(searchExpression,                            
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true}
                                                      });

            var result = new SearchUsersResult();
            result.QueryTime = queryResult.Header.QTime;
            result.Count = queryResult.NumFound;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;

            pager.TotalItems = result.Count;

            foreach (var resultItem in queryResult)
                result.UserIds.Add(resultItem.Id);

            return result;
        }
    }
}