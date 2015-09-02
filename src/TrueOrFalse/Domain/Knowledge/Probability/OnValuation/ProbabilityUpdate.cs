using System.Diagnostics;
using NHibernate.Util;

namespace TrueOrFalse
{
    public class ProbabilityUpdate : IRegisterAsInstancePerLifetime
    {
        private readonly AnswerHistoryRepository _answerHistoryRepository;
        private readonly QuestionValuationRepo _questionValuationRepo;
        private readonly ProbabilityCalc _probabilityCalc;
        private readonly QuestionRepository _questionRepo;
        private readonly UserRepo _userRepo;

        public ProbabilityUpdate(
            AnswerHistoryRepository answerHistoryRepository,
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc probabilityCalc, 
            QuestionRepository questionRepo,
            UserRepo userRepo)
        {
            _answerHistoryRepository = answerHistoryRepository;
            _questionValuationRepo = questionValuationRepo;
            _probabilityCalc = probabilityCalc;
            _questionRepo = questionRepo;
            _userRepo = userRepo;
        }

        public void Run(int userId)
        {
            _questionValuationRepo.GetByUser(userId, onlyActiveKnowledge:false).ForEach(Run);
        }

        public void Run(int questionId, int userId)
        {
            if (userId == -1)
                return;

            var questionValuation =
                _questionValuationRepo.GetBy(questionId, userId) ??
                    new QuestionValuation
                    {
                        Question = _questionRepo.GetById(questionId), 
                        User = _userRepo.GetById(userId)
                    };

            Run(questionValuation);
        }

        private void Run(QuestionValuation questionValuation)
        {
            var sp = Stopwatch.StartNew();

            var questionId = questionValuation.Question.Id;
            var userId = questionValuation.User.Id;

            var answerHistoryItems = _answerHistoryRepository.GetBy(questionId, userId);
            int correctnessProbability = _probabilityCalc.Run(answerHistoryItems);

            questionValuation.CorrectnessProbability = correctnessProbability;

            if (answerHistoryItems.Count <= 4)
                questionValuation.KnowledgeStatus = KnowledgeStatus.Unknown;
            else 
                questionValuation.KnowledgeStatus = 
                    correctnessProbability >= 89 ? 
                        KnowledgeStatus.Secure : 
                        KnowledgeStatus.Weak;

            _questionValuationRepo.CreateOrUpdate(questionValuation);

            Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, questionId, userId);
        }
    }
}