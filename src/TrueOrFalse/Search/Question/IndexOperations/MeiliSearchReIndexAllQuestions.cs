﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meilisearch;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class MeiliSearchReIndexAllQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly MeilisearchClient _client;
        private readonly QuestionRepo _questionRepo;
        private QuestionValuationRepo __questionValuationRepo;

        private QuestionValuationRepo _questionValuationRepo
        {
            get
            {
                if (__questionValuationRepo == null)
                    __questionValuationRepo = Sl.Resolve<QuestionValuationRepo>();

                return __questionValuationRepo;
            }
        }

        public MeiliSearchReIndexAllQuestions()
        {
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
        }

        public async Task Go()
        {
            await _client.DeleteIndexAsync(MeiliSearchKonstanten.Questions);
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