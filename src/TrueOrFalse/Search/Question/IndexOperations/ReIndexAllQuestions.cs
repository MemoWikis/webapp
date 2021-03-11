using System.Linq;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepo _questionRepo;
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

        public ReIndexAllQuestions(QuestionRepo questionRepo, ISolrOperations<QuestionSolrMap> solrOperations)
        {
            _questionRepo = questionRepo;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all questions
            _solrOperations.Commit();

            var allQuestionValuations = _questionValuationRepo.GetAll();

            foreach (var question in _questionRepo.GetAll().Where(q => !q.IsWorkInProgress))
                _solrOperations.Add(
                    ToQuestionSolrMap.Run(question, 
                        allQuestionValuations
                            .Where(v => v.Question.Id == question.Id)
                            .Select(qv => qv.ToCacheItem())
                            .ToList())
                );

            _solrOperations.Commit();
        }
    }
}