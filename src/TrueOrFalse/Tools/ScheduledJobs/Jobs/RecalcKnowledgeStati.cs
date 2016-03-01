using Autofac;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                foreach (var user in scope.Resolve<UserRepo>().GetAll())
                    scope.Resolve<ProbabilityUpdate_Valuation>().Run(user.Id);
            }, "RecalcKnowledgeStati");
        }
    }
}