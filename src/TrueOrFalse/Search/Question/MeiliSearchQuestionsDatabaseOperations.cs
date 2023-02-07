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
        public static async Task<TaskInfo> CreateAsync(Question question, string indexConstant = MeiliSearchKonstanten.Questions)
        {
            try
            {
                var questionMapAndIndex = CreateQuestionMap(question, indexConstant, out var index);
                return await index.AddDocumentsAsync(new List<MeiliSearchQuestionMap> { questionMapAndIndex })
                .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot create question in MeiliSearch", e);
            }
            return null;
        }

        /// <summary>
        /// Update MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> UpdateAsync(Question question, string indexConstant = MeiliSearchKonstanten.Questions)
        {
            try
            {
                var QuestionMapAndIndex = CreateQuestionMap(question, indexConstant, out var index);
                return await index.UpdateDocumentsAsync(new List<MeiliSearchQuestionMap> { QuestionMapAndIndex })
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated question in MeiliSearch", e);
            }
            return null;
        }

        /// <summary>
        /// Delete MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> DeleteAsync(Question question, string indexConstant = MeiliSearchKonstanten.Questions)
        {
            try
            {
                var QuestionMapAndIndex = CreateQuestionMap(question, indexConstant, out var index);
                return await index
                    .DeleteOneDocumentAsync(QuestionMapAndIndex.Id.ToString())
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated question in MeiliSearch", e);
            }
            return null;
        }

        private static MeiliSearchQuestionMap CreateQuestionMap(Question question, string indexConstant, out Index index)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            index = client.Index(indexConstant);
            var questionMap = new MeiliSearchQuestionMap
            {
                CreatorId = question.Creator.Id,
                Description = question.Description ?? "",
                Categories = question.Categories.Select(c => c.Name).ToList(),
                CategoryIds = question.Categories.Select(c => c.Id).ToList(),
                Id = question.Id,
                Solution = question.Solution,
                SolutionType = (int)question.SolutionType,
                Text = question.Text
            };
            return questionMap;
        }
    }
}
