
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                UserCache.CreateItemFromDatabase(dataMap.GetInt("userId"));

            }, "InitUserValuationCache");
        }
    }
}
