public class MeiliReIndexPagesMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchReIndexPages _meilisearchReIndexPages;

    public string OperationName => "MeiliReIndexAllPages";

    public MeiliReIndexPagesMaintenanceOperation(
        MeilisearchReIndexPages meilisearchReIndexPages)
    {
        _meilisearchReIndexPages = meilisearchReIndexPages;
    }

    public async Task Run(string jobId)
    {
        try
        {
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all pages...", "Reindex Pages");
            
            await _meilisearchReIndexPages.Run();
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Pages have been re-indexed.", "Reindex Pages");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", "Reindex Pages");
        }
    }
}