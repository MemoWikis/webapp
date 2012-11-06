namespace TrueOrFalse
{
    public class QuestionDeleter : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerHistoryRepository _answerHistory;

        public QuestionDeleter(QuestionRepository questionRepository, 
                               AnswerHistoryRepository answerHistory)
        {
            _questionRepository = questionRepository;
            _answerHistory = answerHistory;
        }

        public void Run(int questionId)
        {
            _questionRepository.Delete(questionId);
            _answerHistory.DeleteFor(questionId);
        }
    }
}
