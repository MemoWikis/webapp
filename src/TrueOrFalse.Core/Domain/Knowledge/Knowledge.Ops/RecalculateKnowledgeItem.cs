namespace TrueOrFalse.Core
{
    public class RecalculateKnowledgeItem : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;
        private readonly QuestionValuationRepository _questionValuationRepository;
        private readonly CorrectnessProbabilityCalculator _correctnessProbabilityCalculator;

        public RecalculateKnowledgeItem(AnswerHistoryRepository answerHistoryRepository,
                                        QuestionValuationRepository questionValuationRepository,
                                        CorrectnessProbabilityCalculator correctnessProbabilityCalculator)
        {
            _answerHistoryRepository = answerHistoryRepository;
            _questionValuationRepository = questionValuationRepository;
            _correctnessProbabilityCalculator = correctnessProbabilityCalculator;
        }

        public void Run(int questionId, int userId)
        {
            int correctnessProbability = _correctnessProbabilityCalculator.Run(
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
