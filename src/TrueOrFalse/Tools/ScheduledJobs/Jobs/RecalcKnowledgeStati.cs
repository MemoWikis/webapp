using Autofac;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        private readonly ISession _nhibernateSession;
        private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
        private readonly QuestionValuationRepo _questionValuationRepo;
        private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
        private readonly AnswerRepo _answerRepo;
        private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
        private readonly CategoryValuationWritingRepo _categoryValuationWritingRepo;

        public RecalcKnowledgeStati(ISession nhibernateSession,
            CategoryValuationReadingRepo categoryValuationReadingRepo,
            QuestionValuationRepo questionValuationRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo,
            KnowledgeSummaryLoader knowledgeSummaryLoader,
            CategoryValuationWritingRepo categoryValuationWritingRepo)
        {
            _nhibernateSession = nhibernateSession;
            _categoryValuationReadingRepo = categoryValuationReadingRepo;
            _questionValuationRepo = questionValuationRepo;
            _probabilityCalcSimple1 = probabilityCalcSimple1;
            _answerRepo = answerRepo;
            _knowledgeSummaryLoader = knowledgeSummaryLoader;
            _categoryValuationWritingRepo = categoryValuationWritingRepo;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                foreach (var user in scope.Resolve<UserReadingRepo>().GetAll())
                {
                    ProbabilityUpdate_Valuation.Run(user.Id, _nhibernateSession, _questionValuationRepo, _probabilityCalcSimple1, _answerRepo);
                    KnowledgeSummaryUpdate.RunForUser(user.Id,
                        _categoryValuationReadingRepo,
                        _categoryValuationWritingRepo, 
                        _knowledgeSummaryLoader);
                }
            }, "RecalcKnowledgeStati");

            return Task.CompletedTask;
        }
    }
}