using System;
using Meilisearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchCategoryDatabaseOperations
    {
        public static async Task CreateAsync(Category category)
        {
            try
            {
                var categoryMapAndIndex = CreateCategoryMap(category, out var index);
                await index.AddDocumentsAsync(new List<MeiliSearchCategoryMap> { categoryMapAndIndex })
                .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot create category in MeiliSearch", e);
            }
        }

        public static async Task UpdateAsync(Category category)
        {
            try
            {
                var categoryMapAndIndex = CreateCategoryMap(category, out var index);
                await index.UpdateDocumentsAsync(new List<MeiliSearchCategoryMap> { categoryMapAndIndex})
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated category in MeiliSearch", e);
            }
        }

        public static async Task DeleteAsync(Category category)
        {
            try
            {
                var categoryMapAndIndex = CreateCategoryMap(category, out var index);
                await index.DeleteOneDocumentAsync(categoryMapAndIndex.Id.ToString())
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated category in MeiliSearch", e);
            }
        }

        private static MeiliSearchCategoryMap CreateCategoryMap(Category category, out Index index)
        {
           var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            index = client.Index(MeiliSearchKonstanten.Categories);
            var categoryMap = new MeiliSearchCategoryMap
            {
                CreatorId = category.Creator.Id,
                DateCreated = category.DateCreated == DateTime.MinValue ? DateTime.Now : category.DateCreated,
                Description = category.Description ?? "",
                Name = category.Name,
                Id = category.Id,
                QuestionCount = category.CountQuestions
            };
            return categoryMap;
        }
    }
}
