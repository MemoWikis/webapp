namespace TrueOrFalse
{
    public class RecalcQuestionWishItem : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;
        private readonly QuestionValuationRepository _questionValuationRepository;
        private readonly CBForUserCalculator _cbForUserCalculator;

        public RecalcQuestionWishItem(
            AnswerHistoryRepository answerHistoryRepository,
            QuestionValuationRepository questionValuationRepository,
            CBForUserCalculator cbForUserCalculator)
        {
            _answerHistoryRepository = answerHistoryRepository;
            _questionValuationRepository = questionValuationRepository;
            _cbForUserCalculator = cbForUserCalculator;
        }

        public void Run(int questionId, int userId)
        {
            int correctnessProbability = _cbForUserCalculator.Run(
                                            _answerHistoryRepository.GetBy(questionId, userId));

            var questionValuation = 
                _questionValuationRepository.GetBy(questionId, userId) ?? new QuestionValuation();

            questionValuation.QuestionId = questionId;
            questionValuation.UserId = userId;
            questionValuation.CorrectnessProbability = correctnessProbability;

            _questionValuationRepository.CreateOrUpdate(questionValuation);
        }
    }
}
