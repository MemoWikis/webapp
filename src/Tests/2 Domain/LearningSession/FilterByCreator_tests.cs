﻿using static LearningSessionCreator;

class FilterByCreator_tests : BaseTestHarness
{
    private static readonly int _userId = 1;
    private readonly QuestionCacheItem _questionUserIsCreator = new() { CreatorId = _userId };
    private readonly QuestionCacheItem _questionUserIsNotCreator = new() { CreatorId = 2 };

    [Test]
    public void Should_Add_UserIsCreator_ConfigHas_CreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = false,
            CurrentUserId = _userId
        };

        var questionProps = FilterByCreator_Test(config, _questionUserIsCreator);

        // CurrentUserId == CreatorId
        Assert.That(questionProps.AddToLearningSession);
    }

    [Test]
    public void Should_Add_UserIsNotCreator_ConfigHas_NotCreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = true,
            CurrentUserId = _userId
        };
        
        var questionProps = FilterByCreator_Test(config, _questionUserIsNotCreator);

        // CurrentUserId != CreatorId
        Assert.That(questionProps.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_AllSelected()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = true,
            CurrentUserId = _userId
        };

        var questionProps = FilterByCreator_Test(config, _questionUserIsCreator);
        Assert.That(questionProps.AddToLearningSession);

        var questionProps2 = FilterByCreator_Test(config, _questionUserIsNotCreator);
        Assert.That(questionProps2.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_NoneSelected()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = false,
            CurrentUserId = _userId
        };

        var questionProps = FilterByCreator_Test(config, _questionUserIsCreator);
        Assert.That(questionProps.AddToLearningSession);

        var questionProps2 = FilterByCreator_Test(config, _questionUserIsNotCreator);
        Assert.That(questionProps2.AddToLearningSession);
    }

    [Test]
    public void ShouldNot_Add_UserIsCreator_ConfigHas_NotCreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = true,
            CurrentUserId = _userId
        };

        var questionProps = FilterByCreator_Test(config, _questionUserIsCreator);
        Assert.That(questionProps.AddToLearningSession, Is.False);
    }

    [Test]
    public void ShouldNot_Add_UserIsNotCreator_ConfigHas_CreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = false,
            CurrentUserId = _userId
        };

        var questionProps = FilterByCreator_Test(config, _questionUserIsNotCreator);
        Assert.That(questionProps.AddToLearningSession, Is.False);
    }

}