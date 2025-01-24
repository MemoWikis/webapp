using Autofac;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcReputationForAll : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                scope.Resolve<JobQueueRepo>().DeleteAllJobs(JobQueueType.UpdateReputationForUser);
                scope.Resolve<UserWritingRepo>().ReputationUpdateForAll();

                return Task.CompletedTask;
            }, "RecalcReputationForAll");

            return Task.CompletedTask;
        }

    }
}