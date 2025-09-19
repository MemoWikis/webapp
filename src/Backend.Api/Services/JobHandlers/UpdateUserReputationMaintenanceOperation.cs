public class UpdateUserReputationMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly UserWritingRepo _userWritingRepo;

    public string OperationName => "UpdateUserReputationAndRankings";

    public UpdateUserReputationMaintenanceOperation(
        UserWritingRepo userWritingRepo)
    {
        _userWritingRepo = userWritingRepo;
    }

    public Task Run(string jobId)
    {
        try
        {
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Updating user reputation and rankings...", OperationName);
            
            _userWritingRepo.ReputationUpdateForAll();
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Reputation and rankings have been updated.", OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
        
        return Task.CompletedTask;
    }
}