using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;

namespace TrueOrFalse.Search
{
    public class MeiliSearchReIndexAllQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepo _questionValuationRepo;
        private readonly QuestionRepo _questionRepo;
        private readonly MeilisearchClient _client;

        public MeiliSearchReIndexAllQuestions(QuestionValuationRepo questionValuationRepo,
            QuestionRepo questionRepo)
        {
            _questionValuationRepo = questionValuationRepo;
            _questionRepo = questionRepo;
        }

        public MeiliSearchReIndexAllQuestions()
        {
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
        }

        public async Task Go()
        {
            var taskId = (await _client.DeleteIndexAsync(MeiliSearchKonstanten.Questions)).TaskUid;
            await _client.WaitForTaskAsync(taskId);

            var allQuestionsFromDb = _questionRepo.GetAll().Where(q => !q.IsWorkInProgress);
            var allValuations = _questionValuationRepo.GetAll();
            var meiliSearchQuestions = new List<MeiliSearchQuestionMap>();

            foreach (var question in allQuestionsFromDb)
            {
                var questionValuations = allValuations
                    .Where(qv => qv.Question.Id == question.Id)
                    .Select(qv => qv.ToCacheItem())
                    .ToList();
                meiliSearchQuestions.Add(MeiliSearchToQuestionMap.Run(question, questionValuations));
            }

            var index = _client.Index(MeiliSearchKonstanten.Questions);
            await index.AddDocumentsAsync(meiliSearchQuestions);
        }
    }
}