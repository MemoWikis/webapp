using Autofac;
using Quartz;

public class MeiliReIndexPagesJob : IJob
{
    public string OperationName => "MeiliReIndexPages";

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var jobTrackingId = dataMap.GetString("jobTrackingId");

        if (string.IsNullOrEmpty(jobTrackingId))
            return;

        await Run(jobTrackingId);
        Log.Information("Job ended - {OperationName}", OperationName);
    }

    private async Task Run(string jobTrackingId)
    {
        await JobExecute.RunAsync(async scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Re-indexing all pages...", OperationName);

                var meilisearchReIndexPages = scope.Resolve<MeilisearchReIndexPages>();
                await meilisearchReIndexPages.Run();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Pages have been re-indexed.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobTrackingId {jobTrackingId}", OperationName, jobTrackingId);
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
            }
        }, OperationName);
    }
}