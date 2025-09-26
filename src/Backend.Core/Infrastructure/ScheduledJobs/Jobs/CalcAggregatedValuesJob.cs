using Autofac;
using Quartz;

public class CalcAggregatedValuesJob : IJob
{
    public string OperationName => "CalcAggregatedValues";

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

                var updateQuestionAnswerCounts = scope.Resolve<UpdateQuestionAnswerCounts>();
                updateQuestionAnswerCounts.Run();

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