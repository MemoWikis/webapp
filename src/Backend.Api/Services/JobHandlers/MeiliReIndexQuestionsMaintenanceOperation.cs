using Microsoft.Extensions.DependencyInjection;

public class MeiliReIndexQuestionsMaintenanceOperation(
    IServiceScopeFactory scopeFactory) : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public string OperationName => "MeiliReIndexAllQuestions";

    public async Task Run(string jobId)
    {
        try
        {
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all questions...", OperationName);
            
            // Create a new scope to get fresh services with new NHibernate session
            using var scope = _scopeFactory.CreateScope();
            var meilisearchReIndexAllQuestions = scope.ServiceProvider.GetRequiredService<MeilisearchReIndexAllQuestions>();
            await meilisearchReIndexAllQuestions.Run();
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Questions have been re-indexed.", OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
    }
}