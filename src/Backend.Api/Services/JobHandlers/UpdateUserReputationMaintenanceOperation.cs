public class UpdateUserReputationMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly UserWritingRepo _userWritingRepo;
    private readonly IMaintenanceJobService _jobService;

    public string OperationName => "UpdateUserReputationAndRankings";

    public UpdateUserReputationMaintenanceOperation(
        UserWritingRepo userWritingRepo,
        IMaintenanceJobService jobService)
    {
        _userWritingRepo = userWritingRepo;
        _jobService = jobService;
    }

    public Task Run(string jobId)
    {
        try
        {
            _jobService.UpdateJobStatus(jobId, JobStatus.Running, "Updating user reputation and rankings...", OperationName);
            
            _userWritingRepo.ReputationUpdateForAll();
            
            _jobService.UpdateJobStatus(jobId, JobStatus.Completed, "Reputation and rankings have been updated.", OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            _jobService.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
        
        return Task.CompletedTask;
    }
}