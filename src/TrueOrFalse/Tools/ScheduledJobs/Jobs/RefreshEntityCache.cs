using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RefreshEntityCache : IJob
    {
        private readonly EntityCacheInitializer _entityCacheInitializer;

        public RefreshEntityCache(EntityCacheInitializer entityCacheInitializer)
        {
            _entityCacheInitializer = entityCacheInitializer;
        }
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                _entityCacheInitializer.Init(" (in JobScheduler) ");
            }, "RefreshEntityCache");
        }

    }
}