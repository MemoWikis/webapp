using Autofac;
using Quartz;
using ISession = NHibernate.ISession;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        private readonly ISession _nhibernateSession;
        private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
        private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
        private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
        private readonly AnswerRepo _answerRepo;
        private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
        private readonly CategoryValuationWritingRepo _categoryValuationWritingRepo;

        public RecalcKnowledgeStati(ISession nhibernateSession,
            CategoryValuationReadingRepo categoryValuationReadingRepo,
            QuestionValuationReadingRepo questionValuationReadingRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo,
            KnowledgeSummaryLoader knowledgeSummaryLoader,
            CategoryValuationWritingRepo categoryValuationWritingRepo)
        {
            _nhibernateSession = nhibernateSession;
            _categoryValuationReadingRepo = categoryValuationReadingRepo;
            _questionValuationReadingRepo = questionValuationReadingRepo;
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
                    new ProbabilityUpdate_Valuation(_nhibernateSession,
                        _questionValuationReadingRepo,
                        _probabilityCalcSimple1,
                        _answerRepo).Run(user.Id);
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