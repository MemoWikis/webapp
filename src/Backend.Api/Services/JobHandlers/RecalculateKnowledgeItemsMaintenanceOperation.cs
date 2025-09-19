public class RecalculateKnowledgeItemsMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly ProbabilityUpdate_ValuationAll _probabilityUpdateValuationAll;
    private readonly ProbabilityUpdate_Question _probabilityUpdateQuestion;
    private readonly PageRepository _pageRepository;
    private readonly AnswerRepo _answerRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;

    public string OperationName => "RecalculateAllKnowledgeItems";

    public RecalculateKnowledgeItemsMaintenanceOperation(
        ProbabilityUpdate_ValuationAll probabilityUpdateValuationAll,
        ProbabilityUpdate_Question probabilityUpdateQuestion,
        PageRepository pageRepository,
        AnswerRepo answerRepo,
        UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo)
    {
        _probabilityUpdateValuationAll = probabilityUpdateValuationAll;
        _probabilityUpdateQuestion = probabilityUpdateQuestion;
        _pageRepository = pageRepository;
        _answerRepo = answerRepo;
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
    }

    public Task Run(string jobId)
    {
        try
        {
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Starting knowledge items recalculation...", OperationName);
            
            _probabilityUpdateValuationAll.Run();
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Updating question probabilities...", OperationName);
            
            _probabilityUpdateQuestion.Run();
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Updating page probabilities...", OperationName);

            new ProbabilityUpdate_Page(_pageRepository, _answerRepo).Run();
            JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Initializing user probability updates...", OperationName);

            ProbabilityUpdate_User.Initialize(_userReadingRepo, _userWritingRepo, _answerRepo);
            ProbabilityUpdate_User.Instance.Run();
            
            JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "Answer probabilities have been recalculated.", OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
        
        return Task.CompletedTask;
    }
}