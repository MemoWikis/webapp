using Meilisearch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchCategoriesDatabaseOperations : MeiliSearchBase
    {
        /// <summary>
        /// Create MeiliSearch Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task CreateAsync(
            Category category,
            string indexConstant = MeiliSearchKonstanten.Categories)
        {
            var categoryMapAndIndex = CreateCategoryMap(category, indexConstant, out var index);
            var taskInfo = await index.AddDocumentsAsync(new List<MeiliSearchCategoryMap>
                    { categoryMapAndIndex })
                .ConfigureAwait(false);
            await CheckStatus(taskInfo);
        }

        /// <summary>
        /// Update MeiliSearch Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task UpdateAsync(
            Category category,
            string indexConstant = MeiliSearchKonstanten.Categories)
        {
            var categoryMapAndIndex = CreateCategoryMap(category, indexConstant, out var index);
            var taskInfo = await index.UpdateDocumentsAsync(new List<MeiliSearchCategoryMap>
                    { categoryMapAndIndex })
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        }

        /// <summary>
        /// Delete MeiliSearch Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task DeleteAsync(
            Category category,
            string indexConstant = MeiliSearchKonstanten.Categories)
        {
            var categoryMapAndIndex = CreateCategoryMap(category, indexConstant, out var index);
            var taskInfo = await index.DeleteOneDocumentAsync(categoryMapAndIndex.Id.ToString())
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        }

        private static MeiliSearchCategoryMap CreateCategoryMap(
            Category category,
            string indexConstant,
            out Meilisearch.Index index)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url,
                MeiliSearchKonstanten.MasterKey);
            index = client.Index(indexConstant);
            var categoryMap = new MeiliSearchCategoryMap
            {
                CreatorId = category.Creator.Id,
                DateCreated = category.DateCreated == DateTime.MinValue
                    ? DateTime.Now
                    : category.DateCreated,
                Description = category.Description ?? "",
                Name = category.Name,
                Id = category.Id,
                QuestionCount = category.CountQuestions
            };
            return categoryMap;
        }
    }
}