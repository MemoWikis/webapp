using System.Collections.Generic;
using SolrNet;
using StackExchange.Profiling;

namespace TrueOrFalse.Search
{
    public class SearchIndexQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<QuestionSolrMap> _solrOperations;

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

        public SearchIndexQuestion(ISolrOperations<QuestionSolrMap> solrOperations)
        {
            _solrOperations = solrOperations;
        }

        public void Update(IList<Question> questions)
        {
            foreach (var question in questions)
            {
                if(question.IsWorkInProgress)
                    continue;

                _solrOperations.Add(ToQuestionSolrMap.Run(
                    question, _questionValuationRepo.GetBy(question.Id)));                
            }
        }

        public void Update(Question question, bool softCommit = true)
        {
            if (question == null)
                return;

            if (question.IsWorkInProgress)
                return;

            if (!softCommit)
            {
                _solrOperations.Add(ToQuestionSolrMap.Run(question, _questionValuationRepo.GetBy(question.Id)));
                _solrOperations.Commit();    
            }
            else
            {
                var solrQuestion = ToQuestionSolrMap.Run(question, _questionValuationRepo.GetBy(question.Id));
                ExecAsync.Go(() => _solrOperations.Add(solrQuestion, new AddParameters {CommitWithin = 5000}));
            }
        }

        public void Delete(Question question)
        {
            _solrOperations.Delete(ToQuestionSolrMap.Run(question, _questionValuationRepo.GetBy(question.Id)));
            _solrOperations.Commit();
        }


    }
}