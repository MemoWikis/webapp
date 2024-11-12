using Meilisearch;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchPagesDatabaseOperations : MeiliSearchBase
    {
        /// <summary>
        /// Create MeiliSearch Category
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task CreateAsync(
            Page page,
            string indexConstant = MeiliSearchConstants.Pages)
        {
            var mapAndIndex = CreatePageMap(page, indexConstant, out var index);
            var taskInfo = await index.AddDocumentsAsync(new List<MeiliSearchPageMap> { mapAndIndex })
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
            string indexConstant = MeiliSearchConstants.Pages)
        {
            var categoryMapAndIndex = CreatePageMap(page, indexConstant, out var index);
            var taskInfo = await index.UpdateDocumentsAsync(new List<MeiliSearchPageMap>
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
            string indexConstant = MeiliSearchConstants.Pages)
        {
            var pageMapIndex = CreatePageMap(page, indexConstant, out var index);
            var taskInfo = await index.DeleteOneDocumentAsync(pageMapIndex.Id.ToString())
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        }

        private static MeiliSearchPageMap CreatePageMap(
            Page page,
            string indexConstant,
            out Meilisearch.Index index)
        {
            var client = new MeilisearchClient(MeiliSearchConstants.Url,
                MeiliSearchConstants.MasterKey);
            index = client.Index(indexConstant);
            var categoryMap = new MeiliSearchPageMap
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