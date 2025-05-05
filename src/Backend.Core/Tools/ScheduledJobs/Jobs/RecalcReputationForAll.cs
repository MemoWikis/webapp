using Autofac;
using Quartz;

public class RecalcReputationForAll : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope =>
        {
            scope.Resolve<JobQueueRepo>().DeleteAllJobs(JobQueueType.UpdateReputationForUser);
            scope.Resolve<UserWritingRepo>().ReputationUpdateForAll();
        }, "RecalcReputationForAll");

        return Task.CompletedTask;
    }
}