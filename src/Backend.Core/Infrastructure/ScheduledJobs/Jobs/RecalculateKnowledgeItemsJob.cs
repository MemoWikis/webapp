using Autofac;
using Quartz;

public class RecalculateKnowledgeItemsJob : IJob
{
    public string OperationName => "RecalculateKnowledgeItems";

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
        await JobExecute.RunAsync(scope =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Starting knowledge items recalculation...", OperationName);

                var probabilityUpdateValuationAll = scope.Resolve<ProbabilityUpdate_ValuationAll>();
                var probabilityUpdateQuestion = scope.Resolve<ProbabilityUpdate_Question>();
                var pageRepository = scope.Resolve<PageRepository>();
                var answerRepo = scope.Resolve<AnswerRepo>();
                var userReadingRepo = scope.Resolve<UserReadingRepo>();
                var userWritingRepo = scope.Resolve<UserWritingRepo>();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Updating valuation for questions...", OperationName);
                probabilityUpdateValuationAll.Run(jobTrackingId);

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Updating correctness probability for questions...", OperationName);
                probabilityUpdateQuestion.Run(jobTrackingId);

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Updating page probabilities...", OperationName);
                new ProbabilityUpdate_Page(pageRepository, answerRepo).Run();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Initializing user probability updates...", OperationName);

                ProbabilityUpdate_User.Initialize(userReadingRepo, userWritingRepo, answerRepo);
                ProbabilityUpdate_User.Instance.Run();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Answer probabilities have been recalculated.", OperationName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to execute {OperationName} with jobTrackingId {jobTrackingId}", OperationName, jobTrackingId);
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
            }

            return Task.CompletedTask;
        }, OperationName);
    }
}