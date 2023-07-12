using Autofac;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        private readonly ISession _nhibernateSession;
        private readonly CategoryValuationRepo _categoryValuationRepo;
        private readonly QuestionValuationRepo _questionValuationRepo;
        private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
        private readonly AnswerRepo _answerRepo;
        private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;

        public RecalcKnowledgeStati(ISession nhibernateSession,
            CategoryValuationRepo categoryValuationRepo,
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo,
            KnowledgeSummaryLoader knowledgeSummaryLoader)
        {
            _nhibernateSession = nhibernateSession;
            _categoryValuationRepo = categoryValuationRepo;
            _questionValuationRepo = questionValuationRepo;
            _probabilityCalcSimple1 = probabilityCalcSimple1;
            _answerRepo = answerRepo;
            _knowledgeSummaryLoader = knowledgeSummaryLoader;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                foreach (var user in scope.Resolve<UserRepo>().GetAll())
                {
                    ProbabilityUpdate_Valuation.Run(user.Id, _nhibernateSession, _questionValuationRepo, _probabilityCalcSimple1, _answerRepo);
                    KnowledgeSummaryUpdate.RunForUser(user.Id, _categoryValuationRepo, _knowledgeSummaryLoader);
                }
            }, "RecalcKnowledgeStati");
        }
    }
}