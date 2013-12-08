using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SearchQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<QuestionSolrMap> _searchOperations;

        public SearchQuestions(ISolrOperations<QuestionSolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchQuestionsResult Run(QuestionSearchSpec searchSpec)
        {
            var orderBy = SearchQuestionsOrderBy.None;
            if (searchSpec.OrderBy.OrderByQuality.IsCurrent()) orderBy = SearchQuestionsOrderBy.Quality;
            else if (searchSpec.OrderBy.OrderByViews.IsCurrent()) orderBy = SearchQuestionsOrderBy.Views;
            else if (searchSpec.OrderBy.OrderByCreationDate.IsCurrent()) orderBy = SearchQuestionsOrderBy.DateCreated;
            else if (searchSpec.OrderBy.OrderByPersonalRelevance.IsCurrent()) orderBy = SearchQuestionsOrderBy.Valuation;

            return Run(
                searchSpec.Filter.SearchTerm,
                searchSpec,
                searchSpec.Filter.CreatorId,
                searchSpec.Filter.ValuatorId,
                orderBy: orderBy
            );
        }

        public SearchQuestionsResult Run(string searchTerm){
            return Run(searchTerm, new Pager());
        }

        public SearchQuestionsResult Run(
            string searchTerm, 
            Pager pager,
            int creatorId = -1, 
            int valuatorId = -1,
            SearchQuestionsOrderBy orderBy = SearchQuestionsOrderBy.None)
        {
            var sqb = new SearchQueryBuilder();

            var categoryFilter = GetCategoryFilter(searchTerm);
            if (categoryFilter != null)
            {
                sqb.Add("Categories", categoryFilter, isMustHave: true, exact: true);
                searchTerm = searchTerm.Replace("Kat:\"" + categoryFilter + "\"","");
            }
                
            else
                sqb.Add("Categories", searchTerm);

            sqb.Add("FullTextStemmed", searchTerm)
                .Add("FullTextExact", searchTerm)
                .Add("CreatorId", creatorId != -1 ? creatorId.ToString() : null, isMustHave: true, exact: true)
                .Add("ValuatorIds", valuatorId != -1 ? valuatorId.ToString() : null, isMustHave: true, exact: true);


            var orderby = new List<SortOrder>();
            if (orderBy == SearchQuestionsOrderBy.Quality)
                orderby.Add(new SortOrder("Quality", Order.DESC));
            else if(orderBy == SearchQuestionsOrderBy.Views)
                orderby.Add(new SortOrder("Views", Order.DESC));
            else if(orderBy == SearchQuestionsOrderBy.Valuation)
                orderby.Add(new SortOrder("Valuation", Order.DESC));
            else if (orderBy == SearchQuestionsOrderBy.DateCreated)
                orderby.Add(new SortOrder("DateCreated", Order.DESC));
            
            var queryResult = _searchOperations.Query(sqb.ToString(),
                                                      new QueryOptions
                                                      {
                                                            Start = pager.LowerBound - 1,
                                                            Rows = pager.PageSize,
                                                            SpellCheck = new SpellCheckingParameters{ Collate = true},
                                                            ExtraParams = new Dictionary<string, string> { { "qt", "dismax" } },
                                                            OrderBy = orderby
                                                      });


            var result = new SearchQuestionsResult();
            result.QueryTime = queryResult.Header.QTime;
            result.Count = queryResult.NumFound;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;
            
            pager.TotalItems = result.Count;

            foreach (var resultItem in queryResult)
                result.QuestionIds.Add(resultItem.Id);

            return result;
        }

        private static string GetCategoryFilter(string searchTerm)
        {
            string categoryFilter = null;
            if (searchTerm != null && searchTerm.IndexOf("Kat:\"") != -1)
            {
                var match = Regex.Match(searchTerm, "Kat:\"(.*)\"", RegexOptions.IgnoreCase);
                if (match.Success)
                    categoryFilter = match.Groups[1].Value;
            }
            return categoryFilter;
        }
    }
}