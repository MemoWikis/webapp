using NUnit.Framework;
using static LearningSessionCreator;

namespace TrueOrFalse.Tests;

class FilterByCreator_tests : BaseTest
{
    public LearningSessionCreator _learningSessionCreator = new LearningSessionCreator();

    [Test]
    public void FilterByCreator_CreatedByCurrentUserAndIsCreator_SetsAddByCreatorTrue()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = false,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 1 };
        var questionProps = new QuestionProperties();

        questionProps = _learningSessionCreator.FilterByCreator_Test(config, question, questionProps);

        // CurrentUserId == CreatorId
        Assert.IsTrue(questionProps.AddToLearningSession);
    }

    [Test]
    public void FilterByCreator_CreatedByCurrentUserAndIsNotCreator_SetsAddByCreatorFalse()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = false,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 2 };
        var questionProps = new QuestionProperties();

        questionProps = _learningSessionCreator.FilterByCreator_Test(config, question, questionProps);

        // CurrentUserId != CreatorId
        Assert.IsFalse(questionProps.AddToLearningSession);
    }

    [Test]
    public void FilterByCreator_NotCreatedByCurrentUserAndIsCreator_SetsAddByCreatorTrue()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = true,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 2 };
        var questionProps = new QuestionProperties();

        questionProps = _learningSessionCreator.FilterByCreator_Test(config, question, questionProps);

        Assert.IsTrue(questionProps.AddToLearningSession);
    }

    [Test]
    public void FilterByCreator_NotCreatedByCurrentUserAndIsNotCreator_SetsAddByCreatorTrue()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = true,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 2 };
        var questionProps = new QuestionProperties();

        questionProps = _learningSessionCreator.FilterByCreator_Test(config, question, questionProps);

        Assert.IsTrue(questionProps.AddToLearningSession);
    }

    [Test]
    public void FilterByCreator_BothFlagsTrueAndIsCreator_SetsAddByCreatorTrue()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = true,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 1 };
        var questionProps = new QuestionProperties();

        questionProps = _learningSessionCreator.FilterByCreator_Test(config, question, questionProps);

        Assert.IsTrue(questionProps.AddToLearningSession);
    }

    [Test]
    public void FilterByCreator_BothFlagsTrueAndIsNotCreator_SetsAddByCreatorTrue()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = true,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 2 };
        var questionProps = new QuestionProperties();

        questionProps = _learningSessionCreator.FilterByCreator_Test(config, question, questionProps);

        Assert.IsTrue(questionProps.AddToLearningSession);
    }

}