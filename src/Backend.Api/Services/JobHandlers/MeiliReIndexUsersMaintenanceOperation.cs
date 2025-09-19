public class MeiliReIndexUsersMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchReIndexUser _meilisearchReIndexUser;

    public string OperationName => "MeiliReIndexAllUsers";

    public MeiliReIndexUsersMaintenanceOperation(
        MeilisearchReIndexUser meilisearchReIndexUser)
    {
        _meilisearchReIndexUser = meilisearchReIndexUser;
    }

    public async Task Run(string jobId)
    {
        try
        {
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all users...", "Reindex Users");
            
            await _meilisearchReIndexUser.RunAll();
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Users have been re-indexed.", "Reindex Users");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", "Reindex Users");
        }
    }
}