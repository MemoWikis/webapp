using Autofac;
using Quartz;

public class MeiliReIndexQuestionsJob : IJob
{
    public string OperationName => "MeiliReIndexAllQuestions";

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
            var meilisearchReIndexAllQuestions = scope.Resolve<MeilisearchReIndexAllQuestions>();

            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Re-indexing all questions...", OperationName);

                await meilisearchReIndexAllQuestions.Run(jobTrackingId);

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Questions have been re-indexed.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobTrackingId {jobTrackingId}", OperationName, jobTrackingId);
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
                throw; // Re-throw to let Quartz handle job failure
            }
        }, OperationName);
    }
}