using static LearningSessionCreator;

class FilterByKnowledgeSummary_tests : BaseTestLegacy
{
    private readonly QuestionValuationCacheItem _questionValuationNotLearned = new() { CorrectnessProbabilityAnswerCount = 0, CorrectnessProbability = new Random().Next(101) };
    private readonly QuestionValuationCacheItem _questionValuationNeedsLearning = new() { CorrectnessProbabilityAnswerCount = 42, CorrectnessProbability = new Random().Next(51) };
    private readonly QuestionValuationCacheItem _questionValuationNeedsConsolidation = new() { CorrectnessProbabilityAnswerCount = 42, CorrectnessProbability = new Random().Next(51, 80) };
    private readonly QuestionValuationCacheItem _questionValuationSolid = new() { CorrectnessProbabilityAnswerCount = 42, CorrectnessProbability = new Random().Next(80, 101) };

    private readonly QuestionCacheItem _question = new() { CorrectnessProbability = 50 };

    [Test]
    public void Should_Add_QuestionIsSolid_ConfigHas_Solid()
    {
        var config = new LearningSessionConfig
        {
            Solid = true,
            NeedsConsolidation = false,
            NeedsLearning = false,
            NotLearned = false
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationSolid);

        Assert.That(questionProperties.Solid);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned, Is.False);

        var expectedProbability = _questionValuationSolid.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_QuestionNeedsConsolidation_ConfigHas_NeedsConsolidation()
    {
        var config = new LearningSessionConfig
        {
            Solid = false,
            NeedsConsolidation = true,
            NeedsLearning = false,
            NotLearned = false
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationNeedsConsolidation);

        Assert.That(questionProperties.Solid, Is.False);
        Assert.That(questionProperties.NeedsConsolidation);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned, Is.False);

        var expectedProbability = _questionValuationNeedsConsolidation.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_QuestionNeedsLearning_ConfigHas_NeedsLearning()
    {
        var config = new LearningSessionConfig
        {
            Solid = false,
            NeedsConsolidation = false,
            NeedsLearning = true,
            NotLearned = false
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationNeedsLearning);

        Assert.That(questionProperties.Solid, Is.False);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning);
        Assert.That(questionProperties.NotLearned, Is.False);

        var expectedProbability = _questionValuationNeedsLearning.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_QuestionNotLearned_ConfigHas_NotLearned()
    {
        var config = new LearningSessionConfig
        {
            Solid = false,
            NeedsConsolidation = false,
            NeedsLearning = false,
            NotLearned = true
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationNotLearned);

        Assert.That(questionProperties.Solid, Is.False);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned);

        var expectedProbability = _questionValuationNotLearned.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_QuestionValuationIsNull_ConfigHas_NotLearned()
    {
        var config = new LearningSessionConfig
        {
            Solid = false,
            NeedsConsolidation = false,
            NeedsLearning = false,
            NotLearned = true
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, null);

        Assert.That(questionProperties.Solid, Is.False);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned);

        var expectedProbability = _question.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_AllSelected()
    {
        var config = new LearningSessionConfig
        {
            Solid = true,
            NeedsConsolidation = true,
            NeedsLearning = true,
            NotLearned = true
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationSolid);

        Assert.That(questionProperties.Solid);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned, Is.False);

        Assert.That(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByKnowledgeSummary_Test(config, _question, questionProperties2, _questionValuationNeedsConsolidation);

        Assert.That(questionProperties2.Solid, Is.False);
        Assert.That(questionProperties2.NeedsConsolidation);
        Assert.That(questionProperties2.NeedsLearning, Is.False);
        Assert.That(questionProperties2.NotLearned, Is.False);

        Assert.That(questionProperties2.AddToLearningSession);

        var questionProperties3 = new QuestionProperties();
        questionProperties3 = FilterByKnowledgeSummary_Test(config, _question, questionProperties3, _questionValuationNeedsLearning);

        Assert.That(questionProperties3.Solid, Is.False);
        Assert.That(questionProperties3.NeedsConsolidation, Is.False);
        Assert.That(questionProperties3.NeedsLearning);
        Assert.That(questionProperties3.NotLearned, Is.False);

        Assert.That(questionProperties3.AddToLearningSession);

        var questionProperties4 = new QuestionProperties();
        questionProperties4 = FilterByKnowledgeSummary_Test(config, _question, questionProperties4, _questionValuationNotLearned);

        Assert.That(questionProperties4.Solid, Is.False);
        Assert.That(questionProperties4.NeedsConsolidation, Is.False);
        Assert.That(questionProperties4.NeedsLearning, Is.False);
        Assert.That(questionProperties4.NotLearned);

        Assert.That(questionProperties4.AddToLearningSession);

        var questionProperties5 = new QuestionProperties();
        questionProperties5 = FilterByKnowledgeSummary_Test(config, _question, questionProperties5, null);

        Assert.That(questionProperties5.Solid, Is.False);
        Assert.That(questionProperties5.NeedsConsolidation, Is.False);
        Assert.That(questionProperties5.NeedsLearning, Is.False);
        Assert.That(questionProperties5.NotLearned);

        Assert.That(questionProperties5.AddToLearningSession);
    }

    [Test]
    public void Should_Add_ConfigHas_NoneSelected()
    {
        var config = new LearningSessionConfig
        {
            Solid = false,
            NeedsConsolidation = false,
            NeedsLearning = false,
            NotLearned = false
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationSolid);

        Assert.That(questionProperties.Solid);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned, Is.False);

        Assert.That(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByKnowledgeSummary_Test(config, _question, questionProperties2, _questionValuationNeedsConsolidation);

        Assert.That(questionProperties2.Solid, Is.False);
        Assert.That(questionProperties2.NeedsConsolidation);
        Assert.That(questionProperties2.NeedsLearning, Is.False);
        Assert.That(questionProperties2.NotLearned, Is.False);

        Assert.That(questionProperties2.AddToLearningSession);

        var questionProperties3 = new QuestionProperties();
        questionProperties3 = FilterByKnowledgeSummary_Test(config, _question, questionProperties3, _questionValuationNeedsLearning);

        Assert.That(questionProperties3.Solid, Is.False);
        Assert.That(questionProperties3.NeedsConsolidation, Is.False);
        Assert.That(questionProperties3.NeedsLearning);
        Assert.That(questionProperties3.NotLearned, Is.False);

        Assert.That(questionProperties3.AddToLearningSession);

        var questionProperties4 = new QuestionProperties();
        questionProperties4 = FilterByKnowledgeSummary_Test(config, _question, questionProperties4, _questionValuationNotLearned);

        Assert.That(questionProperties4.Solid, Is.False);
        Assert.That(questionProperties4.NeedsConsolidation, Is.False);
        Assert.That(questionProperties4.NeedsLearning, Is.False);
        Assert.That(questionProperties4.NotLearned);

        Assert.That(questionProperties4.AddToLearningSession);

        var questionProperties5 = new QuestionProperties();
        questionProperties5 = FilterByKnowledgeSummary_Test(config, _question, questionProperties5, null);

        Assert.That(questionProperties5.Solid, Is.False);
        Assert.That(questionProperties5.NeedsConsolidation, Is.False);
        Assert.That(questionProperties5.NeedsLearning, Is.False);
        Assert.That(questionProperties5.NotLearned);

        Assert.That(questionProperties5.AddToLearningSession);
    }

    [Test]
    public void Should_Add_QuestionIsSolid_ConfigHas_Solid_NotLearned()
    {
        var config = new LearningSessionConfig
        {
            Solid = true,
            NeedsConsolidation = false,
            NeedsLearning = false,
            NotLearned = true
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationSolid);

        Assert.That(questionProperties.Solid);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned, Is.False);

        var expectedProbability = _questionValuationSolid.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession);
    }

    [Test]
    public void ShouldNot_Add_QuestionIsSolid_ConfigHas_NeedsConsolidation_NotLearned()
    {
        var config = new LearningSessionConfig
        {
            Solid = false,
            NeedsConsolidation = true,
            NeedsLearning = false,
            NotLearned = true
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationSolid);

        Assert.That(questionProperties.Solid);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned, Is.False);

        var expectedProbability = _questionValuationSolid.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession, Is.False);
    }

    [Test]
    public void ShouldNot_Add_QuestionIsNotLearned_or_ValuationIsNull_ConfigHas_Solid_NeedsConsolidation_NeedsLearning()
    {
        var config = new LearningSessionConfig
        {
            Solid = true,
            NeedsConsolidation = true,
            NeedsLearning = true,
            NotLearned = false
        };

        var questionProperties = new QuestionProperties();
        questionProperties = FilterByKnowledgeSummary_Test(config, _question, questionProperties, _questionValuationNotLearned);

        Assert.That(questionProperties.Solid, Is.False);
        Assert.That(questionProperties.NeedsConsolidation, Is.False);
        Assert.That(questionProperties.NeedsLearning, Is.False);
        Assert.That(questionProperties.NotLearned);

        var expectedProbability = _questionValuationNotLearned.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.That(questionProperties.AddToLearningSession, Is.False);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByKnowledgeSummary_Test(config, _question, questionProperties2, null);

        Assert.That(questionProperties2.Solid, Is.False);
        Assert.That(questionProperties2.NeedsConsolidation, Is.False);
        Assert.That(questionProperties2.NeedsLearning, Is.False);
        Assert.That(questionProperties2.NotLearned);

        var expectedProbability2 = _question.CorrectnessProbability;
        Assert.That(questionProperties2.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability2));
        Assert.That(questionProperties2.AddToLearningSession, Is.False);
    }
}
