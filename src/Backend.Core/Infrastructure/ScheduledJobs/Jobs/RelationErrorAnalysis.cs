using Autofac;
using Quartz;

public class RelationErrorAnalysis : IJob
{
    public string OperationName => "RelationErrorAnalysis";

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
        await JobExecute.RunAsync(async scope =>
        {

        try
        {
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Analyzing relation errors...", OperationName);

            var _relationErrors = scope.Resolve<RelationErrors>();
            
            var errors = _relationErrors.GetErrors();
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Processing error data...", OperationName);
            
            // Cache the results for later retrieval
            RelationErrorsCache.SetCache(errors);
            
            var errorCount = errors.Data?.Count ?? 0;
            var message = errorCount > 0 
                ? $"Analysis complete. Found {errorCount} pages with relation errors."
                : "Analysis complete. No relation errors found.";
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, message, OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
        
        }, OperationName);
    }
}