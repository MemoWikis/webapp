using Meilisearch;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchCategoriesDatabaseOperations : MeiliSearchBase
    {
        /// <summary>
        /// Create MeiliSearch Category
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task CreateAsync(
            Page page,
            string indexConstant = MeiliSearchConstants.Categories)
        {
            var categoryMapAndIndex = CreateCategoryMap(page, indexConstant, out var index);
            var taskInfo = await index.AddDocumentsAsync(new List<MeiliSearchCategoryMap>
                    { categoryMapAndIndex })
                .ConfigureAwait(false);
            await CheckStatus(taskInfo);
        }

        /// <summary>
        /// Update MeiliSearch Category
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task UpdateAsync(
            Page page,
            string indexConstant = MeiliSearchConstants.Categories)
        {
            var categoryMapAndIndex = CreateCategoryMap(page, indexConstant, out var index);
            var taskInfo = await index.UpdateDocumentsAsync(new List<MeiliSearchCategoryMap>
                    { categoryMapAndIndex })
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        }

        /// <summary>
        /// Delete MeiliSearch Category
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task DeleteAsync(
            Page page,
            string indexConstant = MeiliSearchConstants.Categories)
        {
            var categoryMapAndIndex = CreateCategoryMap(page, indexConstant, out var index);
            var taskInfo = await index.DeleteOneDocumentAsync(categoryMapAndIndex.Id.ToString())
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        }

        private static MeiliSearchCategoryMap CreateCategoryMap(
            Page page,
            string indexConstant,
            out Meilisearch.Index index)
        {
            var client = new MeilisearchClient(MeiliSearchConstants.Url,
                MeiliSearchConstants.MasterKey);
            index = client.Index(indexConstant);
            var categoryMap = new MeiliSearchCategoryMap
            {
                CreatorName = page.Creator.Name,
                DateCreated = page.DateCreated == DateTime.MinValue
                    ? DateTime.Now
                    : page.DateCreated,
                Description = page.Description ?? "",
                Name = page.Name,
                Id = page.Id,
            };
            return categoryMap;
        }
    }
}