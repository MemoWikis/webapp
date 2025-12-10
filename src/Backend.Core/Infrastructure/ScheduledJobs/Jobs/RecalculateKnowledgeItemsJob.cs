using Autofac;
using Quartz;

public class RecalculateKnowledgeItemsJob : IJob
{
    private static volatile bool _interrupted = false;
    
    public string OperationName => "RecalculateKnowledgeItems";
    
    public static void RequestInterrupt()
    {
        _interrupted = true;
        Log.Information("Interrupt requested for {JobName}", nameof(RecalculateKnowledgeItemsJob));
    }
    
    public static void ResetInterrupt()
    {
        _interrupted = false;
    }

    public void Interrupt()
    {
        Log.Information("Interrupt signal received for {OperationName}", OperationName);
        _interrupted = true;
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

        Log.Information("Starting job execution - {OperationName} with jobTrackingId: {jobTrackingId}", OperationName, jobTrackingId);
        await Run(jobTrackingId);
        Log.Information("Job ended - {OperationName}", OperationName);
    }

    private async Task Run(string jobTrackingId)
    {
        Log.Information("RecalculateKnowledgeItemsJob.Run called with jobTrackingId: {jobTrackingId}", jobTrackingId);
        
        await JobExecute.RunAsync(scope =>
        {
            Log.Information("Inside JobExecute.RunAsync lambda for RecalculateKnowledgeItems");
            try
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Starting knowledge items recalculation...", OperationName);

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted before starting", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted", OperationName);
                    return Task.CompletedTask;
                }

                var probabilityUpdateValuationAll = scope.Resolve<ProbabilityUpdate_ValuationAll>();
                var probabilityUpdateQuestion = scope.Resolve<ProbabilityUpdate_Question>();
                var pageRepository = scope.Resolve<PageRepository>();
                var answerRepo = scope.Resolve<AnswerRepo>();
                var userReadingRepo = scope.Resolve<UserReadingRepo>();
                var userWritingRepo = scope.Resolve<UserWritingRepo>();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Starting batch processing of question valuations...", OperationName);
                probabilityUpdateValuationAll.Run(jobTrackingId);

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted after valuation update", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted during valuation processing", OperationName);
                    return Task.CompletedTask;
                }

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Starting batch processing of question probabilities...", OperationName);
                probabilityUpdateQuestion.Run(jobTrackingId);

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted after question probability update", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted during question processing", OperationName);
                    return Task.CompletedTask;
                }

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Starting batch processing of page probabilities...", OperationName);
                new ProbabilityUpdate_Page(pageRepository, answerRepo).Run(jobTrackingId);

                if (_interrupted)
                {
                    Log.Information("Job {OperationName} was interrupted after page probability update", OperationName);
                    JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Failed, "Job was interrupted during page processing", OperationName);
                    return Task.CompletedTask;
                }

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Initializing user probability updates...", OperationName);

                ProbabilityUpdate_User.Initialize(userReadingRepo, userWritingRepo, answerRepo);
                ProbabilityUpdate_User.Instance.Run();

                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Completed, "Answer probabilities have been recalculated.", OperationName);
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