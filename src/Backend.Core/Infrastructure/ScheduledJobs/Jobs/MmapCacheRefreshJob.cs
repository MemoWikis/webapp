using Quartz;
using System.Diagnostics;

public class MmapCacheRefreshJob : IJob
{
    private readonly MmapCacheRefreshService _mmapCacheRefreshService;

    public MmapCacheRefreshJob(MmapCacheRefreshService mmapCacheRefreshService)
    {
        _mmapCacheRefreshService = mmapCacheRefreshService;
    }

    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope =>
        {
            _mmapCacheRefreshService.TriggerManualRefresh();
        }, "MmapCacheRefreshJob");

        return Task.CompletedTask;
    }
}