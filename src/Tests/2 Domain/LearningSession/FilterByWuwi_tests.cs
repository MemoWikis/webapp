using static LearningSessionCreator;

class FilterByWuwi_tests : BaseTestHarness
{
    private readonly QuestionValuationCacheItem _questionValuationIsInWishknowledge = new() { IsInWishKnowledge = true };
    private readonly QuestionValuationCacheItem _questionValuationIsNotInWishknowledge = new() { IsInWishKnowledge = false };

    [Test]
    public void Should_Add_IsInWuwi_ConfigHas_InWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.That(questionProperties.InWuwi);
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_IsNotInWuwi_ConfigHas_NotInWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties);

        Assert.That(questionProperties.NotInWuwi);
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_AllSelected()
    {
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.That(questionProperties.InWuwi);
        Assert.That(questionProperties.NotInWuwi, Is.False);
        Assert.That(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties2);

        Assert.That(questionProperties2.InWuwi, Is.False);
        Assert.That(questionProperties2.NotInWuwi);
        Assert.That(questionProperties2.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_NoneSelected()
    {
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.That(questionProperties.InWuwi);
        Assert.That(questionProperties.NotInWuwi, Is.False);
        Assert.That(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties2);

        Assert.That(questionProperties2.InWuwi, Is.False);
        Assert.That(questionProperties2.NotInWuwi);
        Assert.That(questionProperties2.AddToLearningSession);
    }


    [Test]
    public void ShouldNot_Add_IsInWuwi_ConfigHas_NotInWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = false, NotInWuwi = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsInWishknowledge, config, questionProperties);

        Assert.That(questionProperties.InWuwi);
        Assert.That(questionProperties.AddToLearningSession, Is.False);
    }

    [Test]
    public void ShouldNot_Add_IsNotInWuwi_ConfigHas_InWuwi()
    {
        var config = new LearningSessionConfig { InWuwi = true, NotInWuwi = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByWuwi_Test(_questionValuationIsNotInWishknowledge, config, questionProperties);

        Assert.That(questionProperties.NotInWuwi);
        Assert.That(questionProperties.AddToLearningSession, Is.False);
    }
}