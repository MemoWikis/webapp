using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search
{
    public class MeiliSearchCategories : IRegisterAsInstancePerLifetime
    {
        private List<CategoryCacheItem> _categories = new();
        private int _count = 20;
        private MeiliSearchCategoriesResult _result;
        public async Task<ISearchCategoriesResult> RunAsync(
            string searchTerm)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            var index = client.Index(MeiliSearchKonstanten.Categories);
            _result = new MeiliSearchCategoriesResult();

            _result.CategoryIds.AddRange(await LoadSearchResults(searchTerm, index));

            return _result;
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

            var categoryMaps =
                (await index.SearchAsync<MeiliSearchCategoryMap>(searchTerm, sq))
                .Hits;

            _result.Count = categoryMaps.Count;

            var categoryMapsSkip = categoryMaps
             .Skip(_count - 20)
             .ToList();

            FilterCacheItems(categoryMapsSkip);

            if (IsReloadRequired(categoryMaps.Count, _categories.Count()))
            {
                _count += 20;
                await LoadSearchResults(searchTerm, index);
            };

            return _categories
                .Select(c => c.Id)
                .Take(5)
                .ToList();
        }

        private void FilterCacheItems(List<MeiliSearchCategoryMap> categoryMaps)
        {
            var categoriesTemp = EntityCache.GetCategories(
                    categoryMaps.Select(c => c.Id))
                .Where(PermissionCheck.CanView)
                .ToList();
            _categories.AddRange(categoriesTemp);
           _categories = _categories
                .Distinct()
                .ToList();
        }
    }
}