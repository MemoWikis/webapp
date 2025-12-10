using Autofac;
using Quartz;

public class RelationErrorAnalysis : IJob
{
    private static volatile bool _interrupted = false;
    
    public string OperationName => "RelationErrorAnalysis";
    
    public static void RequestInterrupt()
    {
        _interrupted = true;
        Log.Information("Interrupt requested for {JobName}", nameof(RelationErrorAnalysis));
    }
    
    public static void ResetInterrupt()
    {
        _interrupted = false;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var jobTrackingId = dataMap.GetString("jobTrackingId");

        if (string.IsNullOrEmpty(jobTrackingId))
        {
            Log.Error("Job {OperationName} cannot execute: jobTrackingId is missing or empty", OperationName);
            return;
        }

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

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted before starting", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted", OperationName);
                    return;
                }

                var _relationErrors = scope.Resolve<RelationErrors>();

                var errors = _relationErrors.GetErrors();

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted after error analysis", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted", OperationName);
                    return;
                }

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Processing error data...",
                    OperationName);

                // Cache the results for later retrieval
                RelationErrorsCache.SetCache(errors);

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted after caching results", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted", OperationName);
                    return;
                }

                var errorCount = errors.Data?.Count ?? 0;
                var message = errorCount > 0
                    ? $"Analysis complete. Found {errorCount} pages with relation errors."
                    : "Analysis complete. No relation errors found.";

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, message, OperationName);
            }
            catch (Exception ex)
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
                throw;
            }
        }, OperationName);
    }
}