namespace TrueOrFalse
{
    public class QuestionDeleter : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerHistoryRepository _answerHistory;
        private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;

        public QuestionDeleter(
            QuestionRepository questionRepository, 
            AnswerHistoryRepository answerHistory, 
            UpdateQuestionCountForCategory updateQuestionCountForCategory                )
        {
            _questionRepository = questionRepository;
            _answerHistory = answerHistory;
            _updateQuestionCountForCategory = updateQuestionCountForCategory;
        }

        public void Run(int questionId)
        {
            var question = _questionRepository.GetById(questionId);
            _questionRepository.Delete(questionId);
            _updateQuestionCountForCategory.Run(question.Categories);
            _answerHistory.DeleteFor(questionId);
        }
    }
}
