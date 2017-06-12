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
                {
                    ProbabilityUpdate_Valuation.Run(user.Id);
                    scope.Resolve<CategoryValuationRepo>().UpdateKnowledgeStatiForUser(user.Id);
                }
            }, "RecalcKnowledgeStati");
        }
    }
}