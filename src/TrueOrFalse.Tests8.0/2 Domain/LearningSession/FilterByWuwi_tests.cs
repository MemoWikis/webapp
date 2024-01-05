using static LearningSessionCreator;

namespace TrueOrFalse.Tests;

class FilterByWuwi_tests : BaseTest
{
    private readonly QuestionValuationCacheItem _questionValuationIsInWishknowledge = new() { IsInWishKnowledge = true };
    private readonly QuestionValuationCacheItem _questionValuationIsNotInWishknowledge = new() { IsInWishKnowledge = false };

    [Test]
    public void Should_Add_IsInWuwi_ConfigHas_InWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.IsTrue(questionProperties.InWuwi);
        Assert.IsTrue(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_IsNotInWuwi_ConfigHas_NotInWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties);

        Assert.IsTrue(questionProperties.NotInWuwi);
        Assert.IsTrue(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_AllSelected()
    {
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.IsTrue(questionProperties.InWuwi);
        Assert.IsFalse(questionProperties.NotInWuwi);
        Assert.IsTrue(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties2);

        Assert.IsFalse(questionProperties2.InWuwi);
        Assert.IsTrue(questionProperties2.NotInWuwi);
        Assert.IsTrue(questionProperties2.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_NoneSelected()
    {
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.IsTrue(questionProperties.InWuwi);
        Assert.IsFalse(questionProperties.NotInWuwi);
        Assert.IsTrue(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties2);

        Assert.IsFalse(questionProperties2.InWuwi);
        Assert.IsTrue(questionProperties2.NotInWuwi);
        Assert.IsTrue(questionProperties2.AddToLearningSession);
    }


    [Test]
    public void ShouldNot_Add_IsInWuwi_ConfigHas_NotInWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.IsTrue(questionProperties.InWuwi);
        Assert.IsFalse(questionProperties.AddToLearningSession);
    }

    [Test]
    public void ShouldNot_Add_IsNotInWuwi_ConfigHas_InWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties);

        Assert.IsTrue(questionProperties.NotInWuwi);
        Assert.IsFalse(questionProperties.AddToLearningSession);
    }
}