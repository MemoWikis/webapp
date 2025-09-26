using Autofac;
using Quartz;

public class MeiliReIndexUsersJob : IJob
{
    public string OperationName => "MeiliReIndexUsers";

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var jobTrackingId = dataMap.GetString("jobTrackingId");

        if (string.IsNullOrEmpty(jobTrackingId))
        {
            Log.Error("Job {OperationName} cannot execute: jobTrackingId is missing or empty", OperationName);
            return;
        }

        await Run(jobTrackingId);
        Log.Information("Job ended - {OperationName}", OperationName);
    }

    private async Task Run(string jobTrackingId)
    {
        await JobExecute.RunAsync(async scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Re-indexing all users...", OperationName);

                var meilisearchReIndexUser = scope.Resolve<MeilisearchReIndexUser>();
                await meilisearchReIndexUser.RunAll();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Users have been re-indexed.", OperationName);
            }
            catch (Exception ex)
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
                throw;
            }
        }, OperationName);
    }
}