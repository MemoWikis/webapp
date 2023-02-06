using System;
using Meilisearch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchQuestionsDatabaseOperations
    {
        /// <summary>
        /// Create MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static async Task CreateAsync(Question question)
        {
            try
            {
                var questionMapAndIndex = CreateQuestionMap(question, out var index);
                await index.AddDocumentsAsync(new List<MeiliSearchQuestionMap> { questionMapAndIndex })
                .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot create question in MeiliSearch", e);
            }
        }

        /// <summary>
        /// Update MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static async Task UpdateAsync(Question question)
        {
            try
            {
                var QuestionMapAndIndex = CreateQuestionMap(question, out var index);
                await index.UpdateDocumentsAsync(new List<MeiliSearchQuestionMap> { QuestionMapAndIndex})
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated question in MeiliSearch", e);
            }
        }

        /// <summary>
        /// Delete MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static async Task DeleteAsync(Question question)
        {
            try
            {
                var QuestionMapAndIndex = CreateQuestionMap(question, out var index);
                await index
                    .DeleteOneDocumentAsync(QuestionMapAndIndex.Id.ToString())
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated question in MeiliSearch", e);
            }
        }

        private static MeiliSearchQuestionMap CreateQuestionMap(Question question, out Index index)
        {
           var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            index = client.Index(MeiliSearchKonstanten.Questions);
            var questionMap = new MeiliSearchQuestionMap
            {
                CreatorId = question.Creator.Id,
                Description = question.Description ?? "",
                Categories = question.Categories.Select(c => c.Name).ToList(),
                CategoryIds = question.Categories.Select(c=> c.Id).ToList(),
                Id = question.Id,
                Solution = question.Solution,
                SolutionType = (int)question.SolutionType,
                Text = question.Text
            };
            return questionMap;
        }
    }
}
