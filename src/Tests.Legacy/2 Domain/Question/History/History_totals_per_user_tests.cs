using System;
using Autofac;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class History_totals_per_user_tests : BaseTest
{
    [Test]
    public void Run()
    {
        var userWritingReo = R<UserWritingRepo>(); 
        var contextUsers = ContextRegisteredUser.New(R<UserReadingRepo>(), userWritingReo).Add().Add().Persist();
        var entityCacheInitilizer = R<EntityCacheInitializer>(); 
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(), 
                R<AnswerRepo>(), 
                R<AnswerQuestion>(),
                userWritingReo, 
                R<CategoryRepository>())
            .AddQuestion(questionText: "QuestionA", solutionText: "AnswerA").AddCategory("A", entityCacheInitilizer)
            .AddQuestion(questionText: "QuestionB", solutionText: "QuestionB").AddCategory("A", entityCacheInitilizer).
            Persist();

        var createdQuestion1 = contextQuestion.All[0];
        var createdQuestion2 = contextQuestion.All[1];
        var user1 = contextUsers.Users[0];
            
        R<AnswerQuestion>().Run(createdQuestion1.Id, "Some answer 1", user1.Id, Guid.NewGuid(), 1, -1);
        R<AnswerQuestion>().Run(createdQuestion1.Id, "Some answer 2", user1.Id, Guid.NewGuid(), 1, -1);
        R<AnswerQuestion>().Run(createdQuestion1.Id, "AnswerA", user1.Id, Guid.NewGuid(), 1, -1);

        var totalsPerUserLoader = R<TotalsPersUserLoader>();
        var totalsResult = totalsPerUserLoader.Run(user1.Id, new[] { createdQuestion1.Id, createdQuestion2.Id});

        Assert.That(totalsResult.ByQuestionId(createdQuestion1.Id).TotalFalse, Is.EqualTo(2));
        Assert.That(totalsResult.ByQuestionId(createdQuestion1.Id).TotalTrue, Is.EqualTo(1));
    }
}