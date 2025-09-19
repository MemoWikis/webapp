using Autofac;
using Quartz;

public class CalcAggregatedValuesJob : IJob
{
    public string OperationName => "CalcAggregatedValues";

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
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Calculating aggregated values for questions...", OperationName);
                
                var updateQuestionAnswerCounts = scope.Resolve<UpdateQuestionAnswerCounts>();
                updateQuestionAnswerCounts.Run();
                
                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Aggregated values have been updated.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
                JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
            }
            
            return Task.CompletedTask;
        }, OperationName);
    }
}