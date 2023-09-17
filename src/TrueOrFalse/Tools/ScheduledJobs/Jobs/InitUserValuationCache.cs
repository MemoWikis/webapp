using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class InitUserValuationCache : IJob
    {
        private readonly SessionUserCache _sessionUserCache;

        public InitUserValuationCache(SessionUserCache sessionUserCache)
        {
            _sessionUserCache = sessionUserCache;
        }
        public Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Logg.r.Information("job started");

                var dataMap = context.JobDetail.JobDataMap;

                _sessionUserCache.CreateSessionUserItemFromDatabase(dataMap.GetInt("userId"));

            }, "InitUserValuationCache");

            return Task.CompletedTask;
        }
    }
}
