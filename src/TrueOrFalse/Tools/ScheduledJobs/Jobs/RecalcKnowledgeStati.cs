using Autofac;
using Quartz;
using ISession = NHibernate.ISession;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        private readonly ISession _nhibernateSession;
        private readonly PageValuationReadingRepository _pageValuationReadingRepository;
        private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
        private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
        private readonly AnswerRepo _answerRepo;
        private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
        private readonly PageValuationWritingRepo _pageValuationWritingRepo;

        public RecalcKnowledgeStati(ISession nhibernateSession,
            PageValuationReadingRepository pageValuationReadingRepository,
            QuestionValuationReadingRepo questionValuationReadingRepo,
            ProbabilityCalc_Simple1 probabilityCalcSimple1,
            AnswerRepo answerRepo,
            KnowledgeSummaryLoader knowledgeSummaryLoader,
            PageValuationWritingRepo pageValuationWritingRepo)
        {
            _nhibernateSession = nhibernateSession;
            _pageValuationReadingRepository = pageValuationReadingRepository;
            _questionValuationReadingRepo = questionValuationReadingRepo;
            _probabilityCalcSimple1 = probabilityCalcSimple1;
            _answerRepo = answerRepo;
            _knowledgeSummaryLoader = knowledgeSummaryLoader;
            _pageValuationWritingRepo = pageValuationWritingRepo;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                foreach (var user in scope.Resolve<UserReadingRepo>().GetAll())
                {
                    new ProbabilityUpdate_Valuation(
                        _nhibernateSession,
                        _questionValuationReadingRepo,
                        _probabilityCalcSimple1,
                        _answerRepo
                        )
                        .Run(user.Id);

                    KnowledgeSummaryUpdate.RunForUser(
                        user.Id,
                        _pageValuationReadingRepository,
                        _pageValuationWritingRepo,
                        _knowledgeSummaryLoader);
                }
                return Task.CompletedTask;
            }, "RecalcKnowledgeStati");

            return Task.CompletedTask;
        }
    }
}