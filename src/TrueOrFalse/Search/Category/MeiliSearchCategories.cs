using System;
using System.Collections.Generic;
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
        private List<CategoryCacheItem> _categories = new List<CategoryCacheItem>();
        private List<MeiliSearchCategoryMap> _categoryMaps; 
        private int _count= 20;
        public async Task<ISearchCategoriesResult> RunAsync(
            string searchTerm,
            Pager pager,
            SearchCategoriesOrderBy orderBy = SearchCategoriesOrderBy.None)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            var index = client.Index(MeiliSearchKonstanten.Categories);
            var result = new MeiliSearchCategoriesResult();

            result.CategoryIds.AddRange(await LoadSearchResults(searchTerm, index));

            return result;
        }

        private bool IsReloadRequired(int searchResultCount, int categoriesCount)
        {
            if (searchResultCount == _count && categoriesCount < 5)
            {
                return true; 
            }

            return false;
        }

        private async Task<List<int>> LoadSearchResults(string searchTerm, Index index)
        {
            var sq = new SearchQuery
            {
                Limit = _count
            };
            _categoryMaps =
                (await index.SearchAsync<MeiliSearchCategoryMap>(searchTerm, sq))
                .Hits
                .ToList();

            var categoriesTemp = EntityCache.GetCategories(
                    _categoryMaps.Select(c => c.Id))
                .Where(PermissionCheck.CanView)
                .ToList();
            _categories.AddRange(categoriesTemp);
            _categories = _categories.Distinct().ToList(); 


            if (IsReloadRequired(_categoryMaps.Count, _categories.Count()))
            {
                _count += 20;
                await LoadSearchResults(searchTerm, index); 
            };

            return _categories.Select(c => c.Id).Take(5).ToList();
        }
    }
}