namespace TrueOrFalse
{
    public class RecalcQuestionWishItem : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;
        private readonly QuestionValuationRepository _questionValuationRepository;
        private readonly ProbabilityForUserCalc _probabilityForUserCalc;

        public RecalcQuestionWishItem(
            AnswerHistoryRepository answerHistoryRepository,
            QuestionValuationRepository questionValuationRepository,
            ProbabilityForUserCalc probabilityForUserCalc)
        {
            _answerHistoryRepository = answerHistoryRepository;
            _questionValuationRepository = questionValuationRepository;
            _probabilityForUserCalc = probabilityForUserCalc;
        }

        public void Run(int questionId, int userId)
        {
            int correctnessProbability = _probabilityForUserCalc.Run(
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
