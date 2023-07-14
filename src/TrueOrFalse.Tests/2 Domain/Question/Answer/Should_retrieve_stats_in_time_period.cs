using System;
using Autofac;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_retrieve_stats_in_time_period : BaseTest
{
    [Test]
    public void Run()
    {
        var contextUsers = ContextRegisteredUser.New(R<UserRepo>()).Add().Persist();
        var contextQuestion = ContextQuestion.New(R<QuestionRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserRepo>(), 
                R<CategoryRepository>(), 
                R<QuestionWritingRepo>())
            .AddQuestion(questionText: "Question", solutionText: "Answer")
                .AddCategory("A", LifetimeScope.Resolve<EntityCacheInitializer>()).
            Persist();

        var createdQuestion = contextQuestion.All[0];
        var user = contextUsers.Users[0];

        Resolve<AnswerQuestion>().Run(createdQuestion.Id, "Answer", user.Id, Guid.NewGuid(), 1, -1);
        Resolve<AnswerQuestion>().Run(createdQuestion.Id, "...,", user.Id, Guid.NewGuid(), 1, -1);
        Resolve<AnswerQuestion>().Run(createdQuestion.Id, "...,", user.Id, Guid.NewGuid(), 1, -1);

        var answerStatsInPeriond =
            Resolve<GetAnswerStatsInPeriod>().Run(user.Id, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

        Assert.That(answerStatsInPeriond[0].TotalAnswers, Is.EqualTo(3));
        Assert.That(answerStatsInPeriond[0].TotalFalseAnswers, Is.EqualTo(2));
        Assert.That(answerStatsInPeriond[0].TotalTrueAnswers, Is.EqualTo(1));
    }
}