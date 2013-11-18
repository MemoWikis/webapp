using System.Linq;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepository;
        private readonly ISolrOperations<QuestionSolrMap> _solrOperations;

        private QuestionValuationRepository __questionValuationRepo;

        private QuestionValuationRepository _questionValuationRepo
        {
            get
            {
                if (__questionValuationRepo == null)
                    __questionValuationRepo = Sl.Resolve<QuestionValuationRepository>();

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

            foreach (var question in _questionRepository.GetAll())
                _solrOperations.Add(
                    ToQuestionSolrMap.Run(question, allQuestionValuations.Where(v => v.QuestionId == question.Id))
                );

            _solrOperations.Commit();
        }
    }
}