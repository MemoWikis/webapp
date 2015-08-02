using System.Linq;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepository;
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

        public ReIndexAllQuestions(QuestionRepository questionRepository, ISolrOperations<QuestionSolrMap> solrOperations)
        {
            _questionRepository = questionRepository;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all questions
            _solrOperations.Commit();

            var allQuestionValuations = _questionValuationRepo.GetAll();

            foreach (var question in _questionRepository.GetAll().Where(q => !q.IsWorkInProgress))
                _solrOperations.Add(
                    ToQuestionSolrMap.Run(question, allQuestionValuations.Where(v => v.Question.Id == question.Id).ToList())
                );

            _solrOperations.Commit();
        }
    }
}