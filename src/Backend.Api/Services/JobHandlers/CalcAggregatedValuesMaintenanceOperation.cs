public class CalcAggregatedValuesMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly UpdateQuestionAnswerCounts _updateQuestionAnswerCounts;

    public string OperationName => "CalcAggregatedValuesQuestions";

    public CalcAggregatedValuesMaintenanceOperation(
        UpdateQuestionAnswerCounts updateQuestionAnswerCounts)
    {
        _updateQuestionAnswerCounts = updateQuestionAnswerCounts;
    }

    public Task Run(string jobId)
    {
        try
        {
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Calculating aggregated values for questions...", OperationName);
            
            _updateQuestionAnswerCounts.Run();
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Aggregated values have been updated.", OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
        
        return Task.CompletedTask;
    }
}