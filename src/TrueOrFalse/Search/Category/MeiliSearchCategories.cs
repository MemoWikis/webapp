using System;
using System.Threading.Tasks;
using Meilisearch;
using Seedworks.Lib.Persistence;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class MeiliSearchCategories : IRegisterAsInstancePerLifetime
    {

        public async Task<ISearchCategoriesResult> Run(
            string searchTerm,
            Pager pager,
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None )
        {
           
            var index = client.Index(MeiliSearchKonstanten.Categories);
            var test = await index.SearchAsync<MeiliSearchCategoryMap>("Daniel");
        
            

                return new MeiliSearchCategoriesResult();
        }
    }
}