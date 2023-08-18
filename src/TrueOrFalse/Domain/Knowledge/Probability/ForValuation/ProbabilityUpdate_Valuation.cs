using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NHibernate.Util;
using ISession = NHibernate.ISession;

namespace TrueOrFalse
{
    public class ProbabilityUpdate_Valuation
    {
        private readonly ISession _session;
        private readonly QuestionValuationRepo _questionValuationRepo;
        private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
        private readonly AnswerRepo _answerRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProbabilityUpdate_Valuation(ISession session,
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo, 
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _session = session;
            _questionValuationRepo = questionValuationRepo;
            _probabilityCalcSimple1 = probabilityCalcSimple1;
            _answerRepo = answerRepo;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Run(int userId)
        {
           _questionValuationRepo
                .GetByUser(userId, onlyActiveKnowledge: false)
                .ForEach(qv=> Run(qv));
        }

        private void Run(QuestionValuation questionValuation)
        {
            UpdateValuationProbabilitys(questionValuation);

            _questionValuationRepo.CreateOrUpdate(questionValuation);
        }

        public void Run(int questionId, 
            int userId,
            QuestionReadingRepo questionReadingRepo,
            UserReadingRepo userReadingRepo)
        {
            var user = userReadingRepo.GetById(userId);

            if(user == null)
                return;

            Run(EntityCache.GetQuestion(questionId),
                user,
                questionReadingRepo
                );
        }

        public void Run(QuestionCacheItem question,
            User user,
            QuestionReadingRepo questionReadingRepo)
        {
            var questionValuation =
                _questionValuationRepo.GetBy(question.Id, user.Id) ??
                    new QuestionValuation
                    {
                        Question = questionReadingRepo.GetById(question.Id),
                        User = user
                    };

            Run(questionValuation);
        }

        private void UpdateValuationProbabilitys(QuestionValuation questionValuation)
        {
            var question = questionValuation.Question;
            var user = EntityCache.GetUserById(questionValuation.User.Id, _httpContextAccessor, _webHostEnvironment);

            var probabilityResult =  _probabilityCalcSimple1
                .Run(EntityCache.GetQuestionById(question.Id, _httpContextAccessor, _webHostEnvironment)
                , user, _session, _answerRepo);

            questionValuation.CorrectnessProbability = probabilityResult.Probability;
            questionValuation.CorrectnessProbabilityAnswerCount = probabilityResult.AnswerCount;
            questionValuation.KnowledgeStatus = probabilityResult.KnowledgeStatus;
        }
    }
}