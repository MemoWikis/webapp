using System.Diagnostics;
using NHibernate.Util;

public class ProbabilityUpdate_Question : IRegisterAsInstancePerLifetime
{
    private readonly AnswerRepo _ansewRepo;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly QuestionRepo _questionRepo;

    public ProbabilityUpdate_Question(AnswerRepo ansewRepo,
        JobQueueRepo jobQueueRepo,
        QuestionRepo questionRepo)
    {
        _ansewRepo = ansewRepo;
        _jobQueueRepo = jobQueueRepo;
        _questionRepo = questionRepo;
    }
    public void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var question in _questionRepo.GetAll())
            Run(question);

        Logg.r().Information("Calculated all question probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(Question question)
    {
        var answers = _ansewRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        _questionRepo.UpdateFieldsOnly(question);

        question.Categories
            .ForEach(c => KnowledgeSummaryUpdate.ScheduleForCategory(c.Id, _jobQueueRepo));
    }
}