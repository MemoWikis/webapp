using Autofac;

public class MeiliReIndexQuestionsMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly IMaintenanceJobService _jobService;

    public string OperationName => "MeiliReIndexAllQuestions";

    public MeiliReIndexQuestionsMaintenanceOperation(IMaintenanceJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task Run(string jobId)
    {
        try
        {
            _jobService.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all questions...", OperationName);
            
            // Resolve the service fresh inside the task to get a new scope/session
            using var scope = ServiceLocator.GetContainer().BeginLifetimeScope();
            var meilisearchReIndexAllQuestions = scope.Resolve<MeilisearchReIndexAllQuestions>();
            await meilisearchReIndexAllQuestions.Run();
            
            _jobService.UpdateJobStatus(jobId, JobStatus.Completed, "Questions have been re-indexed.", OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            _jobService.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
    }
}