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
            searchSpec.SpellCheck = new SpellCheckResult(result.SpellChecking);

            return result;
        }

        public SearchCategoriesResult Run(
            string searchTerm, 
            bool searchStartingWith = false, 
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None)
        {
            return Run(searchTerm, new Pager(), searchStartingWith, orderBy);
        }

        public SearchCategoriesResult Run(
            string searchTerm,
            Pager pager, 
            bool searchStartingWith = false,
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None)
        {
            var sqb = new SearchQueryBuilder()
                .Add("FullTextStemmed", searchTerm, searchStartingWith)
                .Add("FullTextExact", searchTerm, searchStartingWith);

            var orderby = new List<SortOrder>();
            if (orderBy == SearchCategoriesOrderBy.QuestionCount)
                orderby.Add(new SortOrder("QuestionCount", Order.DESC));
            else if (orderBy == SearchCategoriesOrderBy.DateCreated)
                orderby.Add(new SortOrder("DateCreated", Order.DESC));

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