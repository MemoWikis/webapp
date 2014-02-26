using System;
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

        public SearchSetsResult Run(SetSearchSpec searchSpec)
        {
            var orderBy = SearchSetsOrderBy.None;
            if (searchSpec.OrderBy.CreationDate.IsCurrent()) orderBy = SearchSetsOrderBy.CreationDate;
            else if (searchSpec.OrderBy.ValuationsCount.IsCurrent()) orderBy = SearchSetsOrderBy.ValuationsCount;
            else if (searchSpec.OrderBy.ValuationsAvg.IsCurrent()) orderBy = SearchSetsOrderBy.ValuationsAvg;

            return Run(
                searchSpec.SearchTerm,
                searchSpec,
                searchSpec.Filter.CreatorId.IsActive()
                    ? Convert.ToInt32(searchSpec.Filter.CreatorId.GetValue())
                    : -1,
                searchSpec.Filter.ValuatorId,
                orderBy: orderBy);
        }

        public SearchSetsResult Run(
            string searchTerm, 
            int creatorId = -1,
            int valuatorId = -1){
                return Run(searchTerm, new Pager(), creatorId, valuatorId);
        }

        public SearchSetsResult Run(
            string searchTerm, 
            Pager pager, 
            int creatorId = -1, 
            int valuatorId = -1,
            bool startsWithSearch = false,
            bool exact = false,
            SearchSetsOrderBy orderBy = SearchSetsOrderBy.None)
        {
            var orderby = new List<SortOrder>();
            if (orderBy == SearchSetsOrderBy.CreationDate)
                orderby.Add(new SortOrder("DateCreated", Order.DESC));
            else if (orderBy == SearchSetsOrderBy.ValuationsCount)
            {
                orderby.Add(new SortOrder("ValuationsCount", Order.DESC));
                orderby.Add(new SortOrder("ValuationsAvg", Order.DESC));
            }
            else if (orderBy == SearchSetsOrderBy.ValuationsAvg)
            {
                orderby.Add(new SortOrder("ValuationsAvg", Order.DESC));
                orderby.Add(new SortOrder("ValuationsCount", Order.DESC));
            }

            var sqb = new SearchQueryBuilder()
                .Add("FullTextStemmed", searchTerm, startsWith : startsWithSearch, exact: exact)
                .Add("FullTextExact", searchTerm, startsWith: startsWithSearch, exact: exact)
                .Add("CreatorId", creatorId != -1 ? creatorId.ToString() : null, isAndCondition: true, exact: true)
                .Add("ValuatorIds", valuatorId != -1 ? valuatorId.ToString() : null, isAndCondition: true, exact: true);

            var queryResult = _searchOperations.Query(sqb.ToString(),
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true},
                                                            OrderBy = orderby
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