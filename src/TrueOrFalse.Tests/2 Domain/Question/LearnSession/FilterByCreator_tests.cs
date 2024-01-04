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
        var q = new QuestionCacheItem { CreatorId = 1 };
        var questionDetail = new QuestionDetail();

        _learningSessionCreator.FilterByCreatorTest(config, q, questionDetail);

        Assert.IsTrue(questionDetail.AddByCreator);
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
        var q = new QuestionCacheItem { CreatorId = 2 };
        var questionDetail = new QuestionDetail();

        _learningSessionCreator.FilterByCreatorTest(config, q, questionDetail);

        Assert.IsFalse(questionDetail.AddByCreator);
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
        var q = new QuestionCacheItem { CreatorId = 1 };
        var questionDetail = new QuestionDetail();

        _learningSessionCreator.FilterByCreatorTest(config, q, questionDetail);

        Assert.IsTrue(questionDetail.AddByCreator);
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
        var q = new QuestionCacheItem { CreatorId = 2 };
        var questionDetail = new QuestionDetail();

        _learningSessionCreator.FilterByCreatorTest(config, q, questionDetail);

        Assert.IsTrue(questionDetail.AddByCreator);
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
        var q = new QuestionCacheItem { CreatorId = 1 };
        var questionDetail = new QuestionDetail();

        _learningSessionCreator.FilterByCreatorTest(config, q, questionDetail);

        Assert.IsTrue(questionDetail.AddByCreator);
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
        var q = new QuestionCacheItem { CreatorId = 2 };
        var questionDetail = new QuestionDetail();

        _learningSessionCreator.FilterByCreatorTest(config, q, questionDetail);

        Assert.IsTrue(questionDetail.AddByCreator);
    }

}