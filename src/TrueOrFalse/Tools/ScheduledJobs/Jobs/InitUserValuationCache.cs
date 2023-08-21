
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        private readonly Logg _logg;
        private readonly SessionUserCache _sessionUserCache;

        public InitUserValuationCache(Logg logg,
            SessionUserCache sessionUserCache)
        {
            _logg = logg;
            _sessionUserCache = sessionUserCache;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                _logg.r().Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                _sessionUserCache.CreateItemFromDatabase(dataMap.GetInt("userId"));

            }, "InitUserValuationCache");

            return Task.CompletedTask;
        }
    }
}
