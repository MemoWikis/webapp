using Autofac;
using Quartz;

public class RecalculateKnowledgeItemsJob : IJob
{
    public string OperationName => "RecalculateKnowledgeItems";

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
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Starting knowledge items recalculation...", OperationName);
                
                var probabilityUpdateValuationAll = scope.Resolve<ProbabilityUpdate_ValuationAll>();
                var probabilityUpdateQuestion = scope.Resolve<ProbabilityUpdate_Question>();
                var pageRepository = scope.Resolve<PageRepository>();
                var answerRepo = scope.Resolve<AnswerRepo>();
                var userReadingRepo = scope.Resolve<UserReadingRepo>();
                var userWritingRepo = scope.Resolve<UserWritingRepo>();
                
                probabilityUpdateValuationAll.Run();
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Updating question probabilities...", OperationName);
                
                probabilityUpdateQuestion.Run();
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Updating page probabilities...", OperationName);

                new ProbabilityUpdate_Page(pageRepository, answerRepo).Run();
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Initializing user probability updates...", OperationName);

                ProbabilityUpdate_User.Initialize(userReadingRepo, userWritingRepo, answerRepo);
                ProbabilityUpdate_User.Instance.Run();
                
                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Answer probabilities have been recalculated.", OperationName);
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