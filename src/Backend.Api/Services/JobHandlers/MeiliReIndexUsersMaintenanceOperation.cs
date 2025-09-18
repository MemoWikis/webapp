public class MeiliReIndexUsersMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchReIndexUser _meilisearchReIndexUser;
    private readonly IMaintenanceJobService _jobService;

    public string OperationName => "MeiliReIndexAllUsers";

    public MeiliReIndexUsersMaintenanceOperation(
        MeilisearchReIndexUser meilisearchReIndexUser,
        IMaintenanceJobService jobService)
    {
        _meilisearchReIndexUser = meilisearchReIndexUser;
        _jobService = jobService;
    }

    public async Task Run(string jobId)
    {
        try
        {
            _jobService.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all users...", "Reindex Users");
            
            await _meilisearchReIndexUser.RunAll();
            
            _jobService.UpdateJobStatus(jobId, JobStatus.Completed, "Users have been re-indexed.", "Reindex Users");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            _jobService.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", "Reindex Users");
        }
    }
}