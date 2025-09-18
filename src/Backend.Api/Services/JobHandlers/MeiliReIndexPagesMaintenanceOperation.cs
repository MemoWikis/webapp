public class MeiliReIndexPagesMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchReIndexPages _meilisearchReIndexPages;
    private readonly IMaintenanceJobService _jobService;

    public string OperationName => "MeiliReIndexAllPages";

    public MeiliReIndexPagesMaintenanceOperation(
        MeilisearchReIndexPages meilisearchReIndexPages,
        IMaintenanceJobService jobService)
    {
        _meilisearchReIndexPages = meilisearchReIndexPages;
        _jobService = jobService;
    }

    public async Task Run(string jobId)
    {
        try
        {
            _jobService.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all pages...", "Reindex Pages");
            
            await _meilisearchReIndexPages.Run();
            
            _jobService.UpdateJobStatus(jobId, JobStatus.Completed, "Pages have been re-indexed.", "Reindex Pages");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            _jobService.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", "Reindex Pages");
        }
    }
}