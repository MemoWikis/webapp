using Meilisearch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchQuestionsDatabaseOperations : MeiliSearchBase
    {
        public MeiliSearchQuestionsDatabaseOperations(IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
            : base(httpContextAccessor, webHostEnvironment){ }
        /// <summary>
        /// Create MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public async Task CreateAsync(Question question, string indexConstant = MeiliSearchKonstanten.Questions)
        {
            var questionMapAndIndex = CreateQuestionMap(question, indexConstant, out var index);
            var taskInfo = await index.AddDocumentsAsync(new List<MeiliSearchQuestionMap> { questionMapAndIndex })
             .ConfigureAwait(false);
            await CheckStatus(taskInfo).ConfigureAwait(false); 
        }

        /// <summary>
        /// Update MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Question question, string indexConstant = MeiliSearchKonstanten.Questions)
        {
        
                var QuestionMapAndIndex = CreateQuestionMap(question, indexConstant, out var index);
               var taskInfo =  await index.UpdateDocumentsAsync(new List<MeiliSearchQuestionMap> { QuestionMapAndIndex })
                    .ConfigureAwait(false);
               await CheckStatus(taskInfo).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete MeiliSearch Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Question question, string indexConstant = MeiliSearchKonstanten.Questions)
        {
            
                var QuestionMapAndIndex = CreateQuestionMap(question, indexConstant, out var index);
                 var taskInfo = await index
                    .DeleteOneDocumentAsync(QuestionMapAndIndex.Id.ToString())
                    .ConfigureAwait(false);
                 await CheckStatus(taskInfo).ConfigureAwait(false);
        }

        private MeiliSearchQuestionMap CreateQuestionMap(Question question, string indexConstant, out Meilisearch.Index index)
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
