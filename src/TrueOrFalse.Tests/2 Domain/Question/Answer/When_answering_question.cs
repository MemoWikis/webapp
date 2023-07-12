﻿using System;
using Autofac;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

[Category(TestCategories.Programmer)]
public class When_answering_question : BaseTest
{
    [Test]
    public void It_should_be_create_answer_history_item_and_knowledge_item()
    {
        var contextUsers = ContextRegisteredUser.New(R<UserRepo>()).Add().Persist();
        var contextQuestion = ContextQuestion.New(R<QuestionRepo>(), R<AnswerRepo>(), R<AnswerQuestion>(), R<UserRepo>())
            .AddQuestion(questionText: "Some Question", solutionText: "Some answer")
            .AddCategory("A", LifetimeScope.Resolve<EntityCacheInitializer>()).
            Persist();

        var createdQuestion = contextQuestion.All[0];
        var user = contextUsers.Users[0];
        Resolve<AnswerQuestion>().Run(createdQuestion.Id, "Some answer", user.Id, Guid.NewGuid(), 1, -1);

        var answers = Resolve<AnswerRepo>().GetAll();
        Assert.That(answers.Count, Is.EqualTo(1));
        Assert.That(answers[0].AnswerText, Is.EqualTo("Some answer"));
        Assert.That(answers[0].UserId, Is.EqualTo(user.Id));

        var questionValuationRepo = Resolve<QuestionValuationRepo>();
        Assert.That(questionValuationRepo.GetBy(createdQuestion.Id, user.Id).CorrectnessProbability, Is.EqualTo(100));
    }
}