using Autofac;
using Quartz;

public class CalcAggregatedValuesJob : IJob
{
    private static volatile bool _interrupted = false;
    
    public string OperationName => "CalcAggregatedValues";
    
    public static void RequestInterrupt()
    {
        _interrupted = true;
        Log.Information("Interrupt requested for {JobName}", nameof(CalcAggregatedValuesJob));
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
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Calculating aggregated values for questions...", OperationName);

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted before starting", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted", OperationName);
                    return Task.CompletedTask;
                }

                var updateQuestionAnswerCounts = scope.Resolve<UpdateQuestionAnswerCounts>();
                updateQuestionAnswerCounts.Run();

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted after aggregated values calculation", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted", OperationName);
                    return Task.CompletedTask;
                }

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Aggregated values have been updated.", OperationName);
            }
            catch (Exception ex)
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
                throw;
            }

            return Task.CompletedTask;
        }, OperationName);
    }
}