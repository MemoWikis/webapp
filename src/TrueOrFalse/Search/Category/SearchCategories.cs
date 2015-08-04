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

        public SearchCategoriesResult Run(CategorySearchSpec searchSpec)
        {
            var orderBy = SearchCategoriesOrderBy.None;
            if (searchSpec.OrderBy.QuestionCount.IsCurrent()) orderBy = SearchCategoriesOrderBy.QuestionCount;
            else if (searchSpec.OrderBy.CreationDate.IsCurrent()) orderBy = SearchCategoriesOrderBy.DateCreated;

            var result = Run(searchSpec.SearchTerm, searchSpec, orderBy: orderBy);
            searchSpec.SpellCheck = new SpellCheckResult(result.SpellChecking, searchSpec.SearchTerm);

            return result;
        }

        public SearchCategoriesResult Run(
            string searchTerm, 
            bool searchOnlyWithStartingWith = false, 
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None,
            int pageSize = 10)
        {
            return Run(searchTerm, new Pager { PageSize = pageSize }, searchOnlyWithStartingWith, orderBy);
        }

        public SearchCategoriesResult Run(
            string searchTerm,
            Pager pager, 
            bool searchOnlyWithStartingWith = false,
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None)
        {
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

            var orderby = new List<SortOrder>();
            if (orderBy == SearchCategoriesOrderBy.QuestionCount)
                orderby.Add(new SortOrder("QuestionCount", Order.DESC));
            else if (orderBy == SearchCategoriesOrderBy.DateCreated)
                orderby.Add(new SortOrder("DateCreated", Order.DESC));

            #if DEBUG
                Logg.r().Information("SearchCategories {Query}", sqb.ToString());
            #endif

            var queryResult = _searchOperations.Query(sqb.ToString(),
                new QueryOptions
                {
                    Start = pager.LowerBound - 1,
                    Rows = pager.PageSize,
                    SpellCheck = new SpellCheckingParameters(),
                    OrderBy = orderby
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