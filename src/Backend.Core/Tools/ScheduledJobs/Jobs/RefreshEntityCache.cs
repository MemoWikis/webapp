using Quartz;

public class RefreshEntityCache : IJob
{
    private readonly EntityCacheInitializer _entityCacheInitializer;

    public RefreshEntityCache(EntityCacheInitializer entityCacheInitializer)
    {
        _entityCacheInitializer = entityCacheInitializer;
    }

    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope => 
        {
            _entityCacheInitializer.Init(" (in JobScheduler) ");
        }, "RefreshEntityCache");

        return Task.CompletedTask;
    }
}