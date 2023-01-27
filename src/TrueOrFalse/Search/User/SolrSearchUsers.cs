using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SolrSearchUsers : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<UserSolrMap> _searchOperations;

        public SolrSearchUsers(ISolrOperations<UserSolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchUsersResult Run(UserSearchSpec searchSpec)
        {
            var orderBy = SearchUsersOrderBy.None;
            if (searchSpec.OrderBy.Reputation.IsCurrent()) orderBy = SearchUsersOrderBy.Rank;
            else if (searchSpec.OrderBy.WishCount.IsCurrent()) orderBy = SearchUsersOrderBy.WishCount;

            var result = Run(searchSpec.SearchTerm, searchSpec, orderBy);
            searchSpec.SpellCheck = new SpellCheckResult(result.SpellChecking, searchSpec.SearchTerm);

            return result;
        }

        public SearchUsersResult Run(
            string searchTerm, 
            Pager pager,
            SearchUsersOrderBy orderBy
        )
        {
            var sqb = new SearchQueryBuilder()
                .Add("Name", searchTerm)
                .Add("Name", searchTerm, startsWith: true, boost: 99999);

            var orderby = new List<SortOrder>();
            if (orderBy == SearchUsersOrderBy.Rank)
                orderby.Add(new SortOrder("Rank", Order.ASC));
            else if (orderBy == SearchUsersOrderBy.WishCount)
                orderby.Add(new SortOrder("WishCountQuestions", Order.DESC));

            var queryResult = _searchOperations.Query(sqb.ToString(),
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters(),
                                                            OrderBy = orderby
                                                      });

            var result = new SearchUsersResult();
            result.QueryTime = queryResult.Header.QTime;
            result.Count = (int)queryResult.NumFound;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;

            pager.TotalItems = result.Count;

            foreach (var resultItem in queryResult)
                result.UserIds.Add(resultItem.Id);

            return result;
        }
    }
}