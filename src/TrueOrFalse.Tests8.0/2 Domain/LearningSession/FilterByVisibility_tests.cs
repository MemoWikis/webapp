using static LearningSessionCreator;

namespace TrueOrFalse.Tests;

class FilterByVisibility_tests : BaseTest
{
    private readonly QuestionCacheItem _publicQuestion = new QuestionCacheItem { Visibility = QuestionVisibility.Public };
    private readonly QuestionCacheItem _privateQuestion = new QuestionCacheItem { Visibility = QuestionVisibility.Private };

    [Test]
    public void Should_Add_QuestionIsPublic_ConfigHas_PublicQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = true, PrivateQuestions = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.IsTrue(questionProperties.Public);
        Assert.IsTrue(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_QuestionIsPrivate_ConfigHas_PrivateQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = false, PrivateQuestions = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _privateQuestion, questionProperties);

        Assert.IsTrue(questionProperties.Private);
        Assert.IsTrue(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_AllSelected()
    {
        var config = new LearningSessionConfig { PublicQuestions = true, PrivateQuestions = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.IsTrue(questionProperties.Public);
        Assert.IsFalse(questionProperties.Private);
        Assert.IsTrue(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByVisibility_Test(config, _privateQuestion, questionProperties2);

        Assert.IsTrue(questionProperties2.Private);
        Assert.IsFalse(questionProperties2.Public);
        Assert.IsTrue(questionProperties2.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_NoneSelected()
    {
        var config = new LearningSessionConfig { PublicQuestions = false, PrivateQuestions = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.IsTrue(questionProperties.Public);
        Assert.IsFalse(questionProperties.Private);
        Assert.IsTrue(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByVisibility_Test(config, _privateQuestion, questionProperties2);

        Assert.IsTrue(questionProperties2.Private);
        Assert.IsFalse(questionProperties2.Public);
        Assert.IsTrue(questionProperties2.AddToLearningSession);
    }

    [Test]
    public void ShouldNot_Add_QuestionIsPublic_ConfigHas_PrivateQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = false, PrivateQuestions = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.IsTrue(questionProperties.Public);
        Assert.IsFalse(questionProperties.Private);
        Assert.IsFalse(questionProperties.AddToLearningSession);
    }

    [Test]
    public void ShouldNot_Add_QuestionIsPrivate_ConfigHas_PublicQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = true, PrivateQuestions = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _privateQuestion, questionProperties);

        Assert.IsTrue(questionProperties.Private);
        Assert.IsFalse(questionProperties.Public);
        Assert.IsFalse(questionProperties.AddToLearningSession);
    }
}