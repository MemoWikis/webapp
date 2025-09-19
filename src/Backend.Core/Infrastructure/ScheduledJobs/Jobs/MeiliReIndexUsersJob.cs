using Autofac;
using Quartz;

public class MeiliReIndexUsersJob : IJob
{
    public string OperationName => "MeiliReIndexUsers";

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var jobId = dataMap.GetString("jobId");
        
        if (string.IsNullOrEmpty(jobId))
            return;

        await Run(jobId);
        Log.Information("Job ended - {OperationName}", OperationName);
    }

    private async Task Run(string jobId)
    {
        await JobExecute.RunAsync(async scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all users...", OperationName);
                
                var meilisearchReIndexUser = scope.Resolve<MeilisearchReIndexUser>();
                await meilisearchReIndexUser.RunAll();
                
                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Users have been re-indexed.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
                JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
            }
        }, OperationName);
    }
}