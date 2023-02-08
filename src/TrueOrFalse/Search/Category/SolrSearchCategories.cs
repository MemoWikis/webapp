using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NHibernate.Mapping;
using NHibernate.Util;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;
using static System.String;

namespace TrueOrFalse.Search
{
    public class SolrSearchCategories : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<CategorySolrMap> _searchOperations;

        public SolrSearchCategories(ISolrOperations<CategorySolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SolrSearchCategoriesResult Run(
            string searchTerm,
            int valuatorId = -1,
            bool searchOnlyWithStartingWith = false,
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None,
            int pageSize = 10,
            int[] categoriesToFilter = null)
        {
            return Run(
                searchTerm, 
                new Pager { PageSize = pageSize },
                valuatorId,
                searchOnlyWithStartingWith,
                orderBy,
                categoriesToFilter
            );
        }

        public SolrSearchCategoriesResult Run(
            string searchTerm,
            Pager pager,
            int valuatorId = -1,
            bool searchOnlyWithStartingWith = false,
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None,
            int[] categoriesToFilter = null)
        {

            var sqb = new SolrSearchQueryBuilder();

            if (searchOnlyWithStartingWith)
            {
                sqb.Add("FullTextStemmed", searchTerm, startsWith: true)
                   .Add("FullTextExact", searchTerm, startsWith: true)
                   .Add("Name", searchTerm, boost: 1000)
                   .Add("Name", searchTerm, startsWith: true, boost: 99999);
            }
            else
            {
                sqb.Add("FullTextStemmed", searchTerm)
                   .Add("FullTextExact", searchTerm)
                   .Add("FullTextExact", searchTerm, startsWith: true)
                   .Add("Name", searchTerm, boost:1000)
                   .Add("Name", searchTerm, startsWith: true, boost: 99999);
            }

            sqb.Add("ValuatorIds", 
                valuatorId != -1 ? valuatorId.ToString() : null, 
                isAndCondition: true, 
                exact: true);

            var orderby = new List<SortOrder>();
            if (orderBy == SearchCategoriesOrderBy.QuestionCount)
                orderby.Add(new SortOrder("QuestionCount", Order.DESC));
            else if (orderBy == SearchCategoriesOrderBy.DateCreated)
                orderby.Add(new SortOrder("DateCreated", Order.ASC));

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

            var result = new SolrSearchCategoriesResult();
            result.QueryTime = queryResult.Header.QTime;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;


            foreach (var resultItem in queryResult)
            {
                if (categoriesToFilter != null && categoriesToFilter.Any(id => id == resultItem.Id))
                    continue;
                if (PermissionCheck.CanViewCategory(resultItem.Id))
                    result.CategoryIds.Add(resultItem.Id);
            }
            result.Count = result.CategoryIds.Count;
            pager.TotalItems = result.Count;

            return result;
        }       
    }
}