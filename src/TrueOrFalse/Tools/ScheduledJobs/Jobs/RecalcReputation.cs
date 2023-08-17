using Autofac;
using Quartz;
using Rollbar;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcReputation : IJob
    {
        public const int IntervalInSeconds = 2;

        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                List<int> successfullJobIds = new List<int>();
                var jobs = scope.Resolve<JobQueueRepo>().GetReputationUpdateUsers();
                var jobsByUserId = jobs.GroupBy(j => j.JobContent);

                foreach (var userJobs in jobsByUserId)
                {
                    if (Convert.ToInt32(userJobs.Key) == -1)
                    {
                        successfullJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                        continue;
                    }

                    try
                    {
                        scope.Resolve<UserWritingRepo>().ReputationUpdate(scope.Resolve<UserReadingRepo>().GetById(Convert.ToInt32(userJobs.Key)));
                        successfullJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                    }
                    catch (Exception e)
                    {
                        new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, "Error in job RecalcReputation.");
                        RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.Resolve<JobQueueRepo>().DeleteById(successfullJobIds);
                    new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("Job RecalcReputation recalculated reputation for " + successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");

            return Task.CompletedTask; 
        }
    }
}
