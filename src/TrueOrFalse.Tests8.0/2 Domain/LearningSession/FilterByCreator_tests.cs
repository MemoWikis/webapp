using static LearningSessionCreator;

namespace TrueOrFalse.Tests;

class FilterByCreator_tests : BaseTest
{
    [Test]
    public void Should_Add_UserIsCreator_ConfigHas_CreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = false,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 1 };
        var questionProps = new QuestionProperties();

        questionProps = FilterByCreator_Test(config, question, questionProps);

        // CurrentUserId == CreatorId
        Assert.IsTrue(questionProps.AddToLearningSession);
    }

    [Test]
    public void Should_Add_UserIsNotCreator_ConfigHas_NotCreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = true,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 2 };
        var questionProps = new QuestionProperties();

        questionProps = FilterByCreator_Test(config, question, questionProps);

        // CurrentUserId != CreatorId
        Assert.IsTrue(questionProps.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_CreatedByCurrentUser_and_NotCreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = true,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 1 };
        var questionProps = new QuestionProperties();

        questionProps = FilterByCreator_Test(config, question, questionProps);

        Assert.IsTrue(questionProps.AddToLearningSession);

        var question2 = new QuestionCacheItem { CreatorId = 2 };
        var questionProps2 = new QuestionProperties();

        questionProps2 = FilterByCreator_Test(config, question2, questionProps2);

        Assert.IsTrue(questionProps2.AddToLearningSession);
    }

    [Test]
    public void Should_Add_No_ConfigSelection()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = false,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 1 };
        var questionProps = new QuestionProperties();

        questionProps = FilterByCreator_Test(config, question, questionProps);

        Assert.IsTrue(questionProps.AddToLearningSession);

        var config2 = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = false,
            CurrentUserId = 1
        };
        var question2 = new QuestionCacheItem { CreatorId = 2 };
        var questionProps2 = new QuestionProperties();

        questionProps2 = FilterByCreator_Test(config2, question2, questionProps2);

        Assert.IsTrue(questionProps2.AddToLearningSession);
    }

    [Test]
    public void ShouldNot_Add_UserIsCreator_ConfigHas_NotCreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = false,
            NotCreatedByCurrentUser = true,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 1 };
        var questionProps = new QuestionProperties();

        questionProps = FilterByCreator_Test(config, question, questionProps);

        Assert.IsFalse(questionProps.AddToLearningSession);
    }

    [Test]
    public void ShouldNot_Add_UserIsNotCreator_ConfigHas_CreatedByCurrentUser()
    {
        var config = new LearningSessionConfig
        {
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = false,
            CurrentUserId = 1
        };
        var question = new QuestionCacheItem { CreatorId = 2 };
        var questionProps = new QuestionProperties();

        questionProps = FilterByCreator_Test(config, question, questionProps);

        Assert.IsFalse(questionProps.AddToLearningSession);
    }

}