using System.Threading.Tasks;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcReputationForAll : IJob
    {

        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                scope.R<JobQueueRepo>().DeleteAllJobs(JobQueueType.UpdateReputationForUser);
                scope.R<ReputationUpdate>().RunForAll();
            }, "RecalcReputationForAll");

            return Task.CompletedTask;
        }

    }
}