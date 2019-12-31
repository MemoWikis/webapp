using System.Threading.Tasks;
using Autofac;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcKnowledgeStati : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                foreach (var user in scope.Resolve<UserRepo>().GetAll())
                {
                    ProbabilityUpdate_Valuation.Run(user.Id);
                    KnowledgeSummaryUpdate.RunForUser(user.Id);
                }
            }, "RecalcKnowledgeStati");

            return Task.CompletedTask;
        }
    }
}