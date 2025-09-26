using Autofac;
using Quartz;

public class RelationErrorAnalysis : IJob
{
    public string OperationName => "RelationErrorAnalysis";

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
        await JobExecute.RunAsync(async scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Analyzing relation errors...",
                    OperationName);

                var _relationErrors = scope.Resolve<RelationErrors>();

                var errors = _relationErrors.GetErrors();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Processing error data...",
                    OperationName);

                // Cache the results for later retrieval
                RelationErrorsCache.SetCache(errors);

                var errorCount = errors.Data?.Count ?? 0;
                var message = errorCount > 0
                    ? $"Analysis complete. Found {errorCount} pages with relation errors."
                    : "Analysis complete. No relation errors found.";

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, message, OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobTrackingId {jobTrackingId}", OperationName,
                    jobTrackingId);
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
            }
        }, OperationName);
    }
}