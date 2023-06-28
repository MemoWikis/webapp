using NHibernate;
using NHibernate.Util;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_Valuation
    {
        public static void Run(int userId, ISession nhibernateSession)
        {
            Sl.QuestionValuationRepo
                .GetByUser(userId, onlyActiveKnowledge: false)
                .ForEach(qv=> Run(qv, nhibernateSession));
        }

        private static void Run(QuestionValuation questionValuation, ISession nhinbernSession)
        {
            UpdateValuationProbabilitys(questionValuation, nhinbernSession);

            Sl.QuestionValuationRepo.CreateOrUpdate(questionValuation);

            //Logg.r().Information("Calculated probability in {elapsed} for question {questionId} and user {userId}: ", sp.Elapsed, question.Id, user.Id);
        }

        public static void Run(int questionId, int userId, ISession nhibernateSession)
        {
            var user = Sl.UserRepo.GetById(userId);

            if(user == null)
                return;

            Run(EntityCache.GetQuestion(questionId), user, nhibernateSession);
        }

        public static void Run(QuestionCacheItem question, User user, ISession nhibernateSession)
        {
            var questionValuation =
                Sl.QuestionValuationRepo.GetBy(question.Id, user.Id) ??
                    new QuestionValuation
                    {
                        Question = Sl.QuestionRepo.GetById(question.Id),
                        User = user
                    };

            Run(questionValuation, nhibernateSession);
        }

        private static void UpdateValuationProbabilitys(QuestionValuation questionValuation, ISession nhinbernateSession)
        {
            var question = questionValuation.Question;
            var user = EntityCache.GetUserById(questionValuation.User.Id);

            var probabilityResult = Sl.R<ProbabilityCalc_Simple1>().Run(EntityCache.GetQuestionById(question.Id), user, nhinbernateSession);
            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
        }
    }
}