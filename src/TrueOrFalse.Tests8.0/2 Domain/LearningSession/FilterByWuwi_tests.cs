using static LearningSessionCreator;

namespace TrueOrFalse.Tests;

class FilterByWuwi_tests : BaseTest
{
    [Test]
    public void Should_Add_IsInWuwi_ConfigHas_InWuwi()
    {
        var questionValuation = new QuestionValuationCacheItem { IsInWishKnowledge = true };
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = false };
        var questionProperties = new QuestionProperties();

        var result = FilterByWuwi_Test(questionValuation, config, questionProperties);

        Assert.IsTrue(result.InWuwi);
        Assert.IsTrue(result.AddToLearningSession);
    }

    [Test]
    public void Should_Add_IsNotInWuwi_ConfigHas_NotInWuwi()
    {
        var questionValuation = new QuestionValuationCacheItem { IsInWishKnowledge = false };
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = true };
        var questionProperties = new QuestionProperties();

        var result = FilterByWuwi_Test(questionValuation, config, questionProperties);

        Assert.IsTrue(result.NotInWuwi);
        Assert.IsTrue(result.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_InWuwi_and_NotInWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = true };

        var questionValuation = new QuestionValuationCacheItem { IsInWishKnowledge = true };
        var questionProperties = new QuestionProperties();

        var result = FilterByWuwi_Test(questionValuation, config, questionProperties);

        Assert.IsTrue(result.InWuwi);
        Assert.IsFalse(result.NotInWuwi);
        Assert.IsTrue(result.AddToLearningSession);

        var questionValuation2 = new QuestionValuationCacheItem { IsInWishKnowledge = false };
        var questionProperties2 = new QuestionProperties();

        var result2 = FilterByWuwi_Test(questionValuation2, config, questionProperties2);

        Assert.IsFalse(result2.InWuwi);
        Assert.IsTrue(result2.NotInWuwi);
        Assert.IsTrue(result2.AddToLearningSession);
    }

    [Test]
    public void Should_Add_No_ConfigSelection()
    {
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = false };

        var questionValuation = new QuestionValuationCacheItem { IsInWishKnowledge = true };
        var questionProperties = new QuestionProperties();

        var result = FilterByWuwi_Test(questionValuation, config, questionProperties);

        Assert.IsTrue(result.InWuwi);
        Assert.IsFalse(result.NotInWuwi);
        Assert.IsTrue(result.AddToLearningSession);

        var questionValuation2 = new QuestionValuationCacheItem { IsInWishKnowledge = false };
        var questionProperties2 = new QuestionProperties();

        var result2 = FilterByWuwi_Test(questionValuation2, config, questionProperties2);

        Assert.IsFalse(result2.InWuwi);
        Assert.IsTrue(result2.NotInWuwi);
        Assert.IsTrue(result2.AddToLearningSession);
    }
}