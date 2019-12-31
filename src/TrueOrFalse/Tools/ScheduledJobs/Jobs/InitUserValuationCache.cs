using System.Threading.Tasks;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                UserCache.GetItem(dataMap.GetInt("userId"));

            }, "InitUserValuationCache");

            return Task.CompletedTask;
        }

    }
}
