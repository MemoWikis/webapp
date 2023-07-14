﻿using Autofac;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

class Get_questions_from_memory_cache : BaseTest
{
    [Test]
    public void Should_store_questions_into_memory_cache()
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(LifetimeScope.Resolve<CategoryRepository>(),
            R<QuestionRepo>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            R<UserRepo>(), 
            R<QuestionWritingRepo>());
        Assert.That(EntityCache.GetAllQuestions().Count, Is.GreaterThan(4999));
    }

    [Test]
    public void Get_anonymous_learning_session()
    {
        Assert.That(new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<QuestionRepo>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            new LearningSessionConfig(),
            R<UserRepo>(), 
            R<QuestionWritingRepo>()).GetSteps(4000, 4000).Count, Is.EqualTo(4000));
    }
}