using Autofac;
using Quartz;

public class RecalcReputation : IJob
{
    public const int IntervalInSeconds = 10;

    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope =>
        {
            List<int> successfulJobIds = new List<int>();
            var jobs = scope.Resolve<JobQueueRepo>().GetReputationUpdateUsers();
            var jobsByUserId = jobs.GroupBy(j => j.JobContent);

            foreach (var userJobs in jobsByUserId)
            {
                if (Convert.ToInt32(userJobs.Key) == -1)
                {
                    successfulJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                    continue;
                }

                try
                {
                    scope.Resolve<UserWritingRepo>()
                        .ReputationUpdate(scope.Resolve<UserReadingRepo>().GetById(Convert.ToInt32(userJobs.Key)));
                    successfulJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error in job RecalcReputation.");
                }
            }

            //Delete jobs that have been executed successfully
            if (successfulJobIds.Count > 0)
            {
                scope.Resolve<JobQueueRepo>().DeleteById(successfulJobIds);
                Log.Information("Job RecalcReputation recalculated reputation for " + successfulJobIds.Count +
                                   " jobs.");
                successfulJobIds.Clear();
            }
        }, "RecalcReputation");

        return Task.CompletedTask;
    }
}