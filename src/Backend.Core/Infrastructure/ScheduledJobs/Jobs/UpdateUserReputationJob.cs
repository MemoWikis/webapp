using Autofac;
using Quartz;

public class UpdateUserReputationJob : IJob
{
    public string OperationName => "UpdateUserReputation";

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
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Updating user reputation and rankings...", OperationName);

                var userWritingRepo = scope.Resolve<UserWritingRepo>();
                userWritingRepo.ReputationUpdateForAll();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Reputation and rankings have been updated.", OperationName);
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