using System.Diagnostics;

public class ProbabilityUpdate_Question : IRegisterAsInstancePerLifetime
{
    private readonly AnswerRepo _ansewRepo;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly KnowledgeSummaryUpdateService _knowledgeSummaryUpdateService;

    public ProbabilityUpdate_Question(AnswerRepo ansewRepo,
        JobQueueRepo jobQueueRepo, QuestionReadingRepo questionReadingRepo,
        QuestionWritingRepo questionWritingRepo,
        KnowledgeSummaryUpdateService knowledgeSummaryUpdateService)
    {
        _ansewRepo = ansewRepo;
        _jobQueueRepo = jobQueueRepo;
        _questionReadingRepo = questionReadingRepo;
        _questionWritingRepo = questionWritingRepo;
        _knowledgeSummaryUpdateService = knowledgeSummaryUpdateService;
    }

    public void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var question in _questionReadingRepo.GetAll())
            Run(question);

        Log.Information("Calculated all question probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(Question question)
    {
        var answers = _ansewRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        _questionWritingRepo.UpdateFieldsOnly(question);

        // Fire-and-forget: send messages without waiting
        var pageIds = question.Pages.Select(p => p.Id);
        _knowledgeSummaryUpdateService.SchedulePageUpdates(pageIds);
    }
}