using NHibernate;
using NHibernate.Util;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_Valuation
    {
        public static void Run(int userId,
            ISession nhibernateSession,
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo)
        {
           questionValuationRepo
                .GetByUser(userId, onlyActiveKnowledge: false)
                .ForEach(qv=> Run(qv, nhibernateSession,questionValuationRepo, probabilityCalcSimple1, answerRepo));
        }

        private static void Run(QuestionValuation questionValuation, ISession nhinbernSession, QuestionValuationRepo questionValuationRepo,ProbabilityCalc_Simple1 probabilityCalcSimple1, AnswerRepo answerRepo)
        {
            UpdateValuationProbabilitys(questionValuation, nhinbernSession, probabilityCalcSimple1, answerRepo);

            questionValuationRepo.CreateOrUpdate(questionValuation);
        }

        public static void Run(int questionId, 
            int userId, 
            ISession nhibernateSession,
            QuestionReadingRepo questionReadingRepo,
            UserRepo userRepo,
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo)
        {
            var user = userRepo.GetById(userId);

            if(user == null)
                return;

            Run(EntityCache.GetQuestion(questionId), user, nhibernateSession, questionReadingRepo, questionValuationRepo, probabilityCalcSimple1, answerRepo);
        }

        public static void Run(QuestionCacheItem question,
            User user,
            ISession nhibernateSession,
            QuestionReadingRepo questionReadingRepo,
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo)
        {
            var questionValuation =
                questionValuationRepo.GetBy(question.Id, user.Id) ??
                    new QuestionValuation
                    {
                        Question = questionReadingRepo.GetById(question.Id),
                        User = user
                    };

            Run(questionValuation, nhibernateSession, questionValuationRepo, probabilityCalcSimple1,answerRepo);
        }

        private static void UpdateValuationProbabilitys(QuestionValuation questionValuation, ISession nhinbernateSession, ProbabilityCalc_Simple1 probabilityCalcSimple1, AnswerRepo answerRepo)
        {
            var question = questionValuation.Question;
            var user = EntityCache.GetUserById(questionValuation.User.Id);

            var probabilityResult =  probabilityCalcSimple1.Run(EntityCache.GetQuestionById(question.Id), user, nhinbernateSession, answerRepo);
            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
        }
    }
}