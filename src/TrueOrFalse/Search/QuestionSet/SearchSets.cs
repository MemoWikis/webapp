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

            var result = Run(
                searchSpec.SearchTerm,
                searchSpec,
                searchSpec.Filter.CreatorId,
                searchSpec.Filter.ValuatorId,
                orderBy: orderBy);

            searchSpec.SpellCheck = new SpellCheckResult(result.SpellChecking, searchSpec.SearchTerm);

            return result;
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
            bool searchOnlyWithStartingWith = false,
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

            var sqb = new SearchQueryBuilder();

            if (searchOnlyWithStartingWith)
            {
                sqb.Add("FullTextStemmed", searchTerm, startsWith: true)
                   .Add("FullTextExact", searchTerm, startsWith: true);
            }
            else
            {
                sqb.Add("FullTextStemmed", searchTerm)
                   .Add("FullTextExact", searchTerm)
                   .Add("FullTextExact", searchTerm, startsWith: true);
            }

            sqb.Add("CreatorId", creatorId != -1 ? creatorId.ToString() : null, isAndCondition: true, exact: true)
               .Add("ValuatorIds", valuatorId != -1 ? valuatorId.ToString() : null, isAndCondition: true, exact: true);

            #if DEBUG
                Logg.r().Information("SearchSets {Query}", sqb.ToString());
            #endif

            var queryResult = _searchOperations.Query(sqb.ToString(),
                new QueryOptions
                {
                    Start = pager.LowerBound - 1,
                    Rows = pager.PageSize,
                    SpellCheck = new SpellCheckingParameters(),
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