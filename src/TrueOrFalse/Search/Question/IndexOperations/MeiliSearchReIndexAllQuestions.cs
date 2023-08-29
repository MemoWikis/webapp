using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;

namespace TrueOrFalse.Search
{
    public class MeiliSearchReIndexAllQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
        private readonly QuestionReadingRepo _questionReadingRepo;
        private readonly MeilisearchClient _client;

        public MeiliSearchReIndexAllQuestions(QuestionValuationReadingRepo questionValuationReadingRepo,
            QuestionReadingRepo questionReadingRepo)
        {
            _questionValuationReadingRepo = questionValuationReadingRepo;
            _questionReadingRepo = questionReadingRepo;
        }

        public MeiliSearchReIndexAllQuestions()
        {
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
        }

        public async Task Go()
        {
            var taskId = (await _client.DeleteIndexAsync(MeiliSearchKonstanten.Questions)).TaskUid;
            await _client.WaitForTaskAsync(taskId);

            var allQuestionsFromDb = _questionReadingRepo.GetAll().Where(q => !q.IsWorkInProgress);
            var allValuations = _questionValuationReadingRepo.GetAll();
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