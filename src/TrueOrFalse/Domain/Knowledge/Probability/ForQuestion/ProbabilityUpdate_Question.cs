using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NHibernate.Util;

public class ProbabilityUpdate_Question : IRegisterAsInstancePerLifetime
{
    private readonly AnswerRepo _ansewRepo;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;

    public ProbabilityUpdate_Question(AnswerRepo ansewRepo,
        JobQueueRepo jobQueueRepo, QuestionReadingRepo questionReadingRepo,
        QuestionWritingRepo questionWritingRepo)
    {
        _ansewRepo = ansewRepo;
        _jobQueueRepo = jobQueueRepo;
        _questionReadingRepo = questionReadingRepo;
        _questionWritingRepo = questionWritingRepo;
    }
    public void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var question in _questionReadingRepo.GetAll())
            Run(question);

        Logg.r.Information("Calculated all question probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(Question question)
    {
        var answers = _ansewRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        _questionWritingRepo.UpdateFieldsOnly(question);

        question.Categories
            .ForEach(c => KnowledgeSummaryUpdate.ScheduleForCategory(c.Id, _jobQueueRepo));
    }
}