using System;
using Meilisearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchDatabaseOperations
    {
        public static async Task CategoryCreateAsync(Category category)
        {
            try
            {
                var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
                var index = client.Index(MeiliSearchKonstanten.Categories);
                var categoryMap = new MeiliSearchCategoryMap
                {
                    CreatorId = category.Creator.Id,
                    DateCreated = category.DateCreated == DateTime.MinValue ? DateTime.Now : category.DateCreated,
                    Description = category.Description ?? "",
                    Name = category.Name,
                    Id = category.Id,
                    QuestionCount = category.CountQuestions
                };

            await index.AddDocumentsAsync(new List<MeiliSearchCategoryMap> { categoryMap }).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var test = e; 
            }
            
        }
    }
}
