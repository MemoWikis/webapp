using Autofac;
using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        private readonly ISession _nhibernateSession;
        private readonly CategoryValuationRepo _categoryValuationRepo;

        public RecalcKnowledgeStati(ISession nhibernateSession, CategoryValuationRepo categoryValuationRepo )
        {
            _nhibernateSession = nhibernateSession;
            _categoryValuationRepo = categoryValuationRepo;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                foreach (var user in scope.Resolve<UserRepo>().GetAll())
                {
                    ProbabilityUpdate_Valuation.Run(user.Id, _nhibernateSession);
                    KnowledgeSummaryUpdate.RunForUser(user.Id, _categoryValuationRepo);
                }
            }, "RecalcKnowledgeStati");
        }
    }
}