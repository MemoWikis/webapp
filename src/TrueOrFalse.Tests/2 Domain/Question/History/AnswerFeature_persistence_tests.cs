using System;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class AnswerFeature_persistence_tests : BaseTest
{
    [Test]
    public void Should_persist()
    {
        ContextQuestion.New()
            .PersistImmediately()
            .AddQuestion(questionText: "Q1", solutionText: "S1")
                .AddAnswers(5, 0, DateTime.Now.AddDays(-1).Date.AddHours(3))
            .AddQuestion(questionText: "Q2", solutionText: "S2")
                .AddAnswers(3, 0, DateTime.Now.AddDays(-1).Date.AddHours(14))
            .Persist();

        GenerateAnswerFeatures.Run();
        AssignAnswerFeatures.Run();

        RecycleContainer();

        var answerFeatures = Sl.R<AnswerHistoryRepo>().GetAll().SelectMany(q => q.Features).ToList();

        Assert.That(answerFeatures.Count(x => x.Id2 == "Time-00-06"), Is.EqualTo(5));
        Assert.That(answerFeatures.Count(x => x.Id2 == "Time-12-18"), Is.EqualTo(3));
    }
}