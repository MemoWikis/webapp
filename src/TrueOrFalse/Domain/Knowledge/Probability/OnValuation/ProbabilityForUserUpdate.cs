using System.Diagnostics;
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
                _questionValuationRepository.GetBy(questionId, userId) ?? 
                new QuestionValuation{QuestionId = questionId, UserId = userId};

            Run(questionValuation);
        }

        private void Run(QuestionValuation questionValuation)
        {
            var sp = Stopwatch.StartNew();

            var questionId = questionValuation.QuestionId;
            var userId = questionValuation.UserId;

            var answerHistoryItems = _answerHistoryRepository.GetBy(questionId, userId);
            int correctnessProbability = _probabilityCalc.Run(answerHistoryItems);

            questionValuation.QuestionId = questionId;
            questionValuation.UserId = userId;
            questionValuation.CorrectnessProbability = correctnessProbability;

            if (answerHistoryItems.Count <= 4)
                questionValuation.KnowledgeStatus = KnowledgeStatus.Unknown;
            else 
                questionValuation.KnowledgeStatus = 
                    correctnessProbability >= 89 ? 
                        KnowledgeStatus.Secure : 
                        KnowledgeStatus.Weak;

            _questionValuationRepository.CreateOrUpdate(questionValuation);

            Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, questionId, userId);
        }
    }
}