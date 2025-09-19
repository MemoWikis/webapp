using Autofac;
using Quartz;

public class MeiliReIndexQuestionsJob : IJob
{
    public string OperationName => "MeiliReIndexAllQuestions";

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
            var meilisearchReIndexAllQuestions = scope.Resolve<MeilisearchReIndexAllQuestions>();
            
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Re-indexing all questions...", OperationName);

                await meilisearchReIndexAllQuestions.Run(jobId);

                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Questions have been re-indexed.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
                JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
                throw; // Re-throw to let Quartz handle job failure
            }
        }, OperationName);
    }
}