using Quartz;
using System.Diagnostics;

public class MmapCacheRefreshJob : IJob
{
    private readonly MmapCacheRefreshService _mmapCacheRefreshService;

    public string OperationName => "RefreshMmapCaches";

    public MmapCacheRefreshJob(MmapCacheRefreshService mmapCacheRefreshService)
    {
        _mmapCacheRefreshService = mmapCacheRefreshService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var jobId = dataMap.GetString("jobId");
        
        if (string.IsNullOrEmpty(jobId))
        {
            // This is the scheduled daily job, no jobId tracking needed
            JobExecute.Run(scope =>
            {
                _mmapCacheRefreshService.TriggerManualRefresh();
            }, "MmapCacheRefreshJob");
        }
        else
        {
            // This is a manually triggered job, use job tracking
            await Run(jobId);
        }
        
        Log.Information("Job ended - {OperationName}", OperationName);
    }

    private async Task Run(string jobId)
    {
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Starting mmap cache refresh...", OperationName);
                
                _mmapCacheRefreshService.TriggerManualRefresh(jobId);
                
                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Mmap caches have been refreshed successfully.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
                JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
            }
            
            return Task.CompletedTask;
        }, OperationName);
    }
}