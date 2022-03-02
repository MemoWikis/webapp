using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Data;
using NHibernate.Util;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_Valuation
    {
        public static void Run(int userId)
        {
            Sl.QuestionValuationRepo
                .GetByUser(userId, onlyActiveKnowledge: false)
                .ForEach(Run);
        }

        private static void Run(QuestionValuation questionValuation)
        {
            UpdateValuationProbabilitys(questionValuation);

            Sl.QuestionValuationRepo.CreateOrUpdate(questionValuation);

            //Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, question.Id, user.Id);
        }

        public static void Run(int questionId, int userId)
        {
            if (userId == -1)
                return;

            Run(EntityCache.GetQuestionCacheItem(questionId), Sl.UserRepo.GetById(userId));
        }

        public static void Run(QuestionCacheItem question, User user)
        {
            var questionValuation =
                Sl.QuestionValuationRepo.GetBy(question.Id, user.Id) ??
                    new QuestionValuation
                    {
                        Question = Sl.QuestionRepo.GetById(question.Id),
                        User = user
                    };

            Run(questionValuation);
        }

        private static void UpdateValuationProbabilitys(QuestionValuation questionValuation)
        {
            var question = questionValuation.Question;
            var user = questionValuation.User;

            var probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(EntityCache.GetQuestionById(question.Id), user);
            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
        }
    }
}