using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate.Util;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_Valuation : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepo _questionValuationRepo;
        private readonly ProbabilityCalc_Simple1 _probabilityCalc;
        private readonly QuestionRepo _questionRepo;
        private readonly UserRepo _userRepo;

        public ProbabilityUpdate_Valuation(
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc_Simple1 probabilityCalc, 
            QuestionRepo questionRepo,
            UserRepo userRepo)
        {
            _questionValuationRepo = questionValuationRepo;
            _probabilityCalc = probabilityCalc;
            _questionRepo = questionRepo;
            _userRepo = userRepo;
        }

        public void Run(int userId)
        {
            _questionValuationRepo
                .GetByUser(userId, onlyActiveKnowledge:false)
                .ForEach(Run);
        }

        public void Run(IEnumerable<Set> sets, int userId)
        {
            sets
                .SelectMany(s => s.QuestionsInSet.Select(qis => qis.Question.Id))
                .ForEach(questionId => Run(questionId, userId));
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

            var question = questionValuation.Question;
            var user = questionValuation.User;

            var probabilityResult = _probabilityCalc.Run(question, user);
            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;

            _questionValuationRepo.CreateOrUpdate(questionValuation);

            Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, question.Id, user.Id);
        }
    }
}