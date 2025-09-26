using Quartz;

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
        var jobTrackingId = dataMap.GetString("jobTrackingId");

        if (string.IsNullOrEmpty(jobTrackingId))
        {
            // This is the scheduled daily job, no jobTrackingId tracking needed
            JobExecute.Run(scope =>
            {
                _mmapCacheRefreshService.TriggerManualRefresh();
            }, "MmapCacheRefreshJob");
        }
        else
        {
            // This is a manually triggered job, use job tracking
            await Run(jobTrackingId);
        }

        Log.Information("Job ended - {OperationName}", OperationName);
    }

    private async Task Run(string jobTrackingId)
    {
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Starting mmap cache refresh...", OperationName);

                _mmapCacheRefreshService.TriggerManualRefresh(jobTrackingId);

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Mmap caches have been refreshed successfully.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobTrackingId {jobTrackingId}", OperationName, jobTrackingId);
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
            }

            return Task.CompletedTask;
        }, OperationName);
    }
}