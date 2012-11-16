using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepository;
        private readonly ISolrOperations<QuestionSolrMap> _solrOperations;

        public ReIndexAllQuestions(
            QuestionRepository questionRepository, 
            ISolrOperations<QuestionSolrMap> solrOperations)
        {
            _questionRepository = questionRepository;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all questions
            _solrOperations.Commit();
            
            foreach (var question in _questionRepository.GetAll())
                _solrOperations.Add(ToQuestionSolrMap.Run(question));

            _solrOperations.Commit();
        }
    }
}