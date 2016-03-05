using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate.Util;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_Valuation : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepo _questionValuationRepo;

        public ProbabilityUpdate_Valuation(QuestionValuationRepo questionValuationRepo)
        {
            _questionValuationRepo = questionValuationRepo;
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
                        Question = Sl.R<QuestionRepo>().GetById(questionId), 
                        User = Sl.R<UserRepo>().GetById(userId)
                    };

            Run(questionValuation);
        }

        private void Run(QuestionValuation questionValuation)
        {
            var sp = Stopwatch.StartNew();

            var question = questionValuation.Question;
            var user = questionValuation.User;

            var probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(question, user);
            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;

            _questionValuationRepo.CreateOrUpdate(questionValuation);

            //Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, question.Id, user.Id);
        }
    }
}