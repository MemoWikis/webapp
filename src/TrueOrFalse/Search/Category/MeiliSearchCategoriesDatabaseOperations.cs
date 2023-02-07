using System;
using Meilisearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchCategoriesDatabaseOperations
    {
        /// <summary>
        /// Create MeiliSearch Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> CreateAsync(Category category, string indexConstant = MeiliSearchKonstanten.Categories)
        {
            try
            {
                var categoryMapAndIndex = CreateCategoryMap(category,indexConstant, out var index);
                return await index.AddDocumentsAsync(new List<MeiliSearchCategoryMap> { categoryMapAndIndex })
                .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot create category in MeiliSearch", e);
            }
            return null;
        }

        /// <summary>
        /// Update MeiliSearch Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> UpdateAsync(Category category, string indexConstant = MeiliSearchKonstanten.Categories)
        {
            try
            {
                var categoryMapAndIndex = CreateCategoryMap(category, indexConstant, out var index);
                return await index.UpdateDocumentsAsync(new List<MeiliSearchCategoryMap> { categoryMapAndIndex})
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated category in MeiliSearch", e);
            }
            return null;
        }

        /// <summary>
        /// Delete MeiliSearch Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> DeleteAsync(Category category, string indexConstant = MeiliSearchKonstanten.Categories)
        {
            try
            {
                var categoryMapAndIndex = CreateCategoryMap(category, indexConstant, out var index);
               return await index.DeleteOneDocumentAsync(categoryMapAndIndex.Id.ToString())
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated category in MeiliSearch", e);
            }
            return null;
        }

        private static MeiliSearchCategoryMap CreateCategoryMap(Category category,string indexConstant, out Index index)
        {
           var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            index = client.Index(indexConstant);
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
