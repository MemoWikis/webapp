public class CalcAggregatedValuesMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly UpdateQuestionAnswerCounts _updateQuestionAnswerCounts;
    private readonly IMaintenanceJobService _jobService;

    public string OperationName => "CalcAggregatedValuesQuestions";

    public CalcAggregatedValuesMaintenanceOperation(
        UpdateQuestionAnswerCounts updateQuestionAnswerCounts,
        IMaintenanceJobService jobService)
    {
        _updateQuestionAnswerCounts = updateQuestionAnswerCounts;
        _jobService = jobService;
    }

    public Task Run(string jobId)
    {
        try
        {
            _jobService.UpdateJobStatus(jobId, JobStatus.Running, "Calculating aggregated values for questions...", OperationName);
            
            _updateQuestionAnswerCounts.Run();
            
            _jobService.UpdateJobStatus(jobId, JobStatus.Completed, "Aggregated values have been updated.", OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            _jobService.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
        
        return Task.CompletedTask;
    }
}