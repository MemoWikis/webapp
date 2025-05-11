using static LearningSessionCreator;

class FilterByVisibility_tests : BaseTestLegacy
{
    private readonly QuestionCacheItem _publicQuestion = new QuestionCacheItem { Visibility = QuestionVisibility.Public };
    private readonly QuestionCacheItem _privateQuestion = new QuestionCacheItem { Visibility = QuestionVisibility.Private };

    [Test]
    public void Should_Add_QuestionIsPublic_ConfigHas_PublicQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = true, PrivateQuestions = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.That(questionProperties.Public, Is.True);
        Assert.That(questionProperties.AddToLearningSession, Is.True);
    }

    [Test]
    public void Should_Add_QuestionIsPrivate_ConfigHas_PrivateQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = false, PrivateQuestions = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _privateQuestion, questionProperties);

        Assert.That(questionProperties.Private, Is.True);
        Assert.That(questionProperties.AddToLearningSession, Is.True);
    }

    [Test]
    public void Should_Add_ConfigHas_AllSelected()
    {
        var config = new LearningSessionConfig { PublicQuestions = true, PrivateQuestions = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.That(questionProperties.Public, Is.True);
        Assert.That(questionProperties.Private, Is.False);
        Assert.That(questionProperties.AddToLearningSession, Is.True);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByVisibility_Test(config, _privateQuestion, questionProperties2);

        Assert.That(questionProperties2.Private, Is.True);
        Assert.That(questionProperties2.Public, Is.False);
        Assert.That(questionProperties2.AddToLearningSession, Is.True);
    }

    [Test]
    public void Should_Add_ConfigHas_NoneSelected()
    {
        var config = new LearningSessionConfig { PublicQuestions = false, PrivateQuestions = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.That(questionProperties.Public, Is.True);
        Assert.That(questionProperties.Private, Is.False);
        Assert.That(questionProperties.AddToLearningSession, Is.True);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByVisibility_Test(config, _privateQuestion, questionProperties2);

        Assert.That(questionProperties2.Private, Is.True);
        Assert.That(questionProperties2.Public, Is.False);
        Assert.That(questionProperties2.AddToLearningSession, Is.True);
    }

    [Test]
    public void ShouldNot_Add_QuestionIsPublic_ConfigHas_PrivateQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = false, PrivateQuestions = true };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _publicQuestion, questionProperties);

        Assert.That(questionProperties.Public, Is.True);
        Assert.That(questionProperties.Private, Is.False);
        Assert.That(questionProperties.AddToLearningSession, Is.False);
    }

    [Test]
    public void ShouldNot_Add_QuestionIsPrivate_ConfigHas_PublicQuestions()
    {
        var config = new LearningSessionConfig { PublicQuestions = true, PrivateQuestions = false };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByVisibility_Test(config, _privateQuestion, questionProperties);

        Assert.That(questionProperties.Private, Is.True);
        Assert.That(questionProperties.Public, Is.False);
        Assert.That(questionProperties.AddToLearningSession, Is.False);
    }
}
