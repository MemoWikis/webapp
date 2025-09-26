using System.Diagnostics;

public class ProbabilityUpdate_Question(
    AnswerRepo _ansewRepo,
    QuestionReadingRepo _questionReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    KnowledgeSummaryUpdateDispatcher _knowledgeSummaryUpdateDispatcher)
    : IRegisterAsInstancePerLifetime
{
    public void Run(string? jobTrackingId = null)
    {
        var sp = Stopwatch.StartNew();

        foreach (var question in _questionReadingRepo.GetAll())
        {
            Run(question);

            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Update question probability for ID {question.Id}...",
                "ProbabilityUpdate_Question");
        }

        Log.Information("Calculated all question probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(Question question)
    {
        var answers = _ansewRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        _questionWritingRepo.UpdateFieldsOnly(question);
        _knowledgeSummaryUpdateDispatcher.SchedulePageUpdatesAsync(question.Pages);
    }
}