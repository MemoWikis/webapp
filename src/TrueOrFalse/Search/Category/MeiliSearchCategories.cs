using Meilisearch;

namespace TrueOrFalse.Search
{
    public class MeiliSearchCategories : MeiliSearchHelper, IRegisterAsInstancePerLifetime
    {
        private List<PageCacheItem> _categories = new();
        private MeiliSearchCategoriesResult _result;
        private readonly PermissionCheck _permissionCheck;
        private int _size;

        /// <summary>
        /// Construktor with optional Parameter size = 5
        /// </summary>
        /// <param name="permissionCheck"></param>
        /// <param name="size"></param>
        public MeiliSearchCategories(PermissionCheck permissionCheck, int size = 5)
        {
            _permissionCheck = permissionCheck;
            _size = size;
        }

        /// <summary>
        /// Get Categories From MeiliSearch Async
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<ISearchCategoriesResult> RunAsync(
            string searchTerm)
        {
            var client = new MeilisearchClient(MeiliSearchConstants.Url, MeiliSearchConstants.MasterKey);
            var index = client.Index(MeiliSearchConstants.Pages);
            _result = new MeiliSearchCategoriesResult();

            _result.CategoryIds.AddRange(await LoadSearchResults(searchTerm, index)
                .ConfigureAwait(false));

            return _result;
        }

        private async Task<List<int>> LoadSearchResults(string searchTerm, Meilisearch.Index index)
        {
            var sq = new SearchQuery { Limit = _count };
            var categoryMaps = (await index.SearchAsync<MeiliSearchCategoryMap>(searchTerm, sq)).Hits;

            _result.Count = categoryMaps.Count;

            var categoryMapsSkip = categoryMaps
                .Skip(_count - 20)
                .ToList();

            FilterCacheItems(categoryMapsSkip);

            if (IsReloadRequired(categoryMaps.Count, _categories.Count()))
            {
                _count += 20;
                await LoadSearchResults(searchTerm, index);
            }

            ;

            return _categories
                .Select(c => c.Id)
                .Take(_size)
                .ToList();
        }

        private void FilterCacheItems(List<MeiliSearchCategoryMap> categoryMaps)
        {
            var categoriesTemp = EntityCache.GetCategories(
                    categoryMaps.Select(c => c.Id))
                .Where(_permissionCheck.CanView)
                .ToList();
            _categories.AddRange(categoriesTemp);
            _categories = _categories
                .Distinct()
                .ToList();
        }
    }
}