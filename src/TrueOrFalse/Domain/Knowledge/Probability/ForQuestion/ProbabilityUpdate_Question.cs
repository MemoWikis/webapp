﻿using System.Diagnostics;
using NHibernate.Util;

public class ProbabilityUpdate_Question : IRegisterAsInstancePerLifetime
{
    private readonly AnswerRepo _ansewRepo;
    private readonly JobQueueRepo _jobQueueRepo;

    public ProbabilityUpdate_Question(AnswerRepo ansewRepo,
        JobQueueRepo jobQueueRepo)
    {
        _ansewRepo = ansewRepo;
        _jobQueueRepo = jobQueueRepo;
    }
    public  void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var question in Sl.R<QuestionRepo>().GetAll())
            Run(question);

        Logg.r().Information("Calculated all question probabilities in {elapsed} ", sp.Elapsed);
    }

    public  void Run(Question question)
    {
        var answers = _ansewRepo.GetByQuestion(question.Id);

        question.CorrectnessProbability = ProbabilityCalc_Question.Run(answers);
        question.CorrectnessProbabilityAnswerCount = answers.Count;

        Sl.QuestionRepo.UpdateFieldsOnly(question);

        question.Categories
            .ForEach(c => KnowledgeSummaryUpdate.ScheduleForCategory(c.Id, _jobQueueRepo));
    }
}