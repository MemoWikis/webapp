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

        if (jobTrackingId == JobScheduler.MmapCacheDailyRefreshTrackingId)
        {
            // This is the scheduled daily job, no jobTrackingId tracking needed
            var dailyRefreshId = JobTracking.CreateJob("RefreshMmapCaches - Daily");
            await Run(dailyRefreshId);
        }
        else
        {
            // This is a manually triggered job, use job tracking
            await Run(jobTrackingId);
        }

        Log.Information("Job ended - {OperationName}", OperationName);
    }

    private async Task Run(string? jobTrackingId = null)
    {
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Starting mmap cache refresh...", OperationName);

                _mmapCacheRefreshService.Refresh(jobTrackingId);

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Mmap caches have been refreshed successfully.", OperationName);
            }
            catch (Exception ex)
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
                throw;
            }

            return Task.CompletedTask;
        }, OperationName);
    }
}