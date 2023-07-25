﻿using System.Linq;
using Autofac;
using NUnit.Framework;
using TrueOrFalse.Tests;

class Count_answer_as_correct : BaseTest
{
    [Test]
    public void SetAnswerAsCorrectAnonymus()
    {
        var learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(), 
            R<AnswerQuestion>(), 
            new LearningSessionConfig(),
            R<UserRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSessionForAnonymusUser(5);

        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.CurrentStep.AnswerState, Is.EqualTo(AnswerState.Correct));
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));
    }

    [Test]
    public void SetAnswerAsCorrectLoggedIn()
    {
        var learningsessionConfig = new LearningSessionConfig
        {
            CurrentUserId = 1,
            MaxQuestionCount = 5,
            CategoryId = 1
        };

        var learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            learningsessionConfig,
            R<UserRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSessionWithUser();
        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(4));

        learningsessionConfig = new LearningSessionConfig
        {
            CurrentUserId = 1,
            IsInTestMode = true,
            MaxQuestionCount = 5,
            CategoryId = 1,
        };
        learningSession = new ContextLearningSession(
            R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            learningsessionConfig,
            R<UserRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSession();

        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));
    }

    [Test]
    public void SetAnswerAsCorrectTestModeAndWishSession()
    {
        var lastUserCacheItem =  ContextQuestion.SetWuwi(1000, 
            R<CategoryValuationRepo>(),
            R<AnswerRepo>(), 
            R<AnswerQuestion>(),
            R<UserRepo>(),
            R<QuestionValuationRepo>(),
            R<CategoryRepository>(), 
            R<QuestionWritingRepo>())
            .Last();

        var learningsessionConfig = new LearningSessionConfig
        {
            CurrentUserId = lastUserCacheItem.Id,
            IsInTestMode = true,
            InWuwi = true,
            MaxQuestionCount = 5,
            CategoryId = 1
        }; 

        var learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            learningsessionConfig,
            R<UserRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSession();

        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));
    }

    [Test]
    public void SetAnswerAsCorrectWishSession(){
        var lastUserCacheItem = ContextQuestion.SetWuwi(10,
            R<CategoryValuationRepo>(), 
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            R<UserRepo>(),
            R<QuestionValuationRepo>(), 
            R<CategoryRepository>(), 
            R<QuestionWritingRepo>())
            .Last();
        var learningSessionConfig = new LearningSessionConfig
        {
            CurrentUserId = ((UserCacheItem)lastUserCacheItem).Id,
            IsInTestMode = false,
            InWuwi = true,
            MaxQuestionCount = 5,
            CategoryId = 1
        }; 
        var learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(), 
            R<AnswerQuestion>(),
            learningSessionConfig, 
            R<UserRepo>(), 
            R<QuestionWritingRepo>()).GetLearningSession(
           );
        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(4));
    }
}