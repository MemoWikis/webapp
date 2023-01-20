using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Meilisearch;
using Seedworks.Lib.Persistence;
using SolrNet;


namespace TrueOrFalse.Search
{
    public class MeiliSearchCategories : IRegisterAsInstancePerLifetime
    {


        public async Task<ISearchCategoriesResult> RunAsync(
            string searchTerm,
            Pager pager,
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            var index = client.Index(MeiliSearchKonstanten.Categories);
            var categoriesSearchResult = await index.SearchAsync<MeiliSearchCategoryMap>(searchTerm);
            var categories = categoriesSearchResult.Hits.ToList();

            var result = new MeiliSearchCategoriesResult();
            result.Count = categories.Count; 

            for (int i = 0; i < pager.PageSize; i++)
            {
                result.CategoryIds.Add(categories[i].Id);
            }

            return result;
        }


      
    }
}