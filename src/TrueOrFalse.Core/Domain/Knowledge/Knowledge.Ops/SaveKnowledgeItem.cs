namespace TrueOrFalse.Core
{
    public class SaveKnowledgeItem : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;
        private readonly KnowledgeItemRepository _knowledgeItemRepository;
        private readonly CorrectnessProbabilityCalculator _correctnessProbabilityCalculator;

        public SaveKnowledgeItem(AnswerHistoryRepository answerHistoryRepository,
                                 KnowledgeItemRepository knowledgeItemRepository,
                                 CorrectnessProbabilityCalculator correctnessProbabilityCalculator)
        {
            _answerHistoryRepository = answerHistoryRepository;
            _knowledgeItemRepository = knowledgeItemRepository;
            _correctnessProbabilityCalculator = correctnessProbabilityCalculator;
        }

        public void Run(int questionId, int userId)
        {
            int correctnessProbability = _correctnessProbabilityCalculator.Run(
                                            _answerHistoryRepository.GetBy(questionId, userId));

            _knowledgeItemRepository.CreateOrUpdate(
                new KnowledgeItem
                {
                    CorrectnessProbability = correctnessProbability,
                    QuestionId = questionId,
                    UserId = userId
                });            
        }
    }
}
