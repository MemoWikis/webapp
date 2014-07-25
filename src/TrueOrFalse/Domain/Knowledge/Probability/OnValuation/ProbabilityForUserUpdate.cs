using NHibernate.Linq;

namespace TrueOrFalse
{
    public class ProbabilityForUserUpdate : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;
        private readonly QuestionValuationRepository _questionValuationRepository;
        private readonly ProbabilityCalc _probabilityCalc;

        public ProbabilityForUserUpdate(
            AnswerHistoryRepository answerHistoryRepository,
            QuestionValuationRepository questionValuationRepository,
            ProbabilityCalc probabilityCalc)
        {
            _answerHistoryRepository = answerHistoryRepository;
            _questionValuationRepository = questionValuationRepository;
            _probabilityCalc = probabilityCalc;
        }

        public void Run(int userId)
        {
            _questionValuationRepository.GetByUser(userId).ForEach(Run);
        }

        public void Run(int questionId, int userId)
        {

            var questionValuation =
                _questionValuationRepository.GetBy(questionId, userId) ?? new QuestionValuation();

            Run(questionValuation);
        }

        private void Run(QuestionValuation questionValuation)
        {
            var questionId = questionValuation.QuestionId;
            var userId = questionValuation.UserId;

            int correctnessProbability = _probabilityCalc.Run(
                                            _answerHistoryRepository.GetBy(questionId, userId));

            questionValuation.QuestionId = questionId;
            questionValuation.UserId = userId;
            questionValuation.CorrectnessProbability = correctnessProbability;

            _questionValuationRepository.CreateOrUpdate(questionValuation);            
        }
    }
}