using Autofac;
using Quartz;

public class UpdateUserReputationJob : IJob
{
    public string OperationName => "UpdateUserReputation";

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
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Updating user reputation and rankings...", OperationName);
                
                var userWritingRepo = scope.Resolve<UserWritingRepo>();
                userWritingRepo.ReputationUpdateForAll();
                
                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Reputation and rankings have been updated.", OperationName);
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