using static LearningSessionCreator;

namespace TrueOrFalse.Tests;

class FilterByKnowledgeSummary_tests : BaseTest
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

        Assert.IsTrue(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsFalse(questionProperties.NotLearned);

        var expectedProbability = _questionValuationSolid.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsTrue(questionProperties.AddToLearningSession);
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

        Assert.IsFalse(questionProperties.Solid);
        Assert.IsTrue(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsFalse(questionProperties.NotLearned);

        var expectedProbability = _questionValuationNeedsConsolidation.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsTrue(questionProperties.AddToLearningSession);
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

        Assert.IsFalse(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsTrue(questionProperties.NeedsLearning);
        Assert.IsFalse(questionProperties.NotLearned);

        var expectedProbability = _questionValuationNeedsLearning.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsTrue(questionProperties.AddToLearningSession);
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

        Assert.IsFalse(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsTrue(questionProperties.NotLearned);

        var expectedProbability = _questionValuationNotLearned.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsTrue(questionProperties.AddToLearningSession);
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

        Assert.IsFalse(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsTrue(questionProperties.NotLearned);

        var expectedProbability = _question.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsTrue(questionProperties.AddToLearningSession);
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

        Assert.IsTrue(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsFalse(questionProperties.NotLearned);

        Assert.IsTrue(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByKnowledgeSummary_Test(config, _question, questionProperties2, _questionValuationNeedsConsolidation);

        Assert.IsFalse(questionProperties2.Solid);
        Assert.IsTrue(questionProperties2.NeedsConsolidation);
        Assert.IsFalse(questionProperties2.NeedsLearning);
        Assert.IsFalse(questionProperties2.NotLearned);

        Assert.IsTrue(questionProperties2.AddToLearningSession);

        var questionProperties3 = new QuestionProperties();
        questionProperties3 = FilterByKnowledgeSummary_Test(config, _question, questionProperties3, _questionValuationNeedsLearning);

        Assert.IsFalse(questionProperties3.Solid);
        Assert.IsFalse(questionProperties3.NeedsConsolidation);
        Assert.IsTrue(questionProperties3.NeedsLearning);
        Assert.IsFalse(questionProperties3.NotLearned);

        Assert.IsTrue(questionProperties3.AddToLearningSession);

        var questionProperties4 = new QuestionProperties();
        questionProperties4 = FilterByKnowledgeSummary_Test(config, _question, questionProperties4, _questionValuationNotLearned);

        Assert.IsFalse(questionProperties4.Solid);
        Assert.IsFalse(questionProperties4.NeedsConsolidation);
        Assert.IsFalse(questionProperties4.NeedsLearning);
        Assert.IsTrue(questionProperties4.NotLearned);

        Assert.IsTrue(questionProperties4.AddToLearningSession);

        var questionProperties5 = new QuestionProperties();
        questionProperties5 = FilterByKnowledgeSummary_Test(config, _question, questionProperties5, null);

        Assert.IsFalse(questionProperties5.Solid);
        Assert.IsFalse(questionProperties5.NeedsConsolidation);
        Assert.IsFalse(questionProperties5.NeedsLearning);
        Assert.IsTrue(questionProperties5.NotLearned);

        Assert.IsTrue(questionProperties5.AddToLearningSession);
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

        Assert.IsTrue(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsFalse(questionProperties.NotLearned);

        Assert.IsTrue(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByKnowledgeSummary_Test(config, _question, questionProperties2, _questionValuationNeedsConsolidation);

        Assert.IsFalse(questionProperties2.Solid);
        Assert.IsTrue(questionProperties2.NeedsConsolidation);
        Assert.IsFalse(questionProperties2.NeedsLearning);
        Assert.IsFalse(questionProperties2.NotLearned);

        Assert.IsTrue(questionProperties2.AddToLearningSession);

        var questionProperties3 = new QuestionProperties();
        questionProperties3 = FilterByKnowledgeSummary_Test(config, _question, questionProperties3, _questionValuationNeedsLearning);

        Assert.IsFalse(questionProperties3.Solid);
        Assert.IsFalse(questionProperties3.NeedsConsolidation);
        Assert.IsTrue(questionProperties3.NeedsLearning);
        Assert.IsFalse(questionProperties3.NotLearned);

        Assert.IsTrue(questionProperties3.AddToLearningSession);

        var questionProperties4 = new QuestionProperties();
        questionProperties4 = FilterByKnowledgeSummary_Test(config, _question, questionProperties4, _questionValuationNotLearned);

        Assert.IsFalse(questionProperties4.Solid);
        Assert.IsFalse(questionProperties4.NeedsConsolidation);
        Assert.IsFalse(questionProperties4.NeedsLearning);
        Assert.IsTrue(questionProperties4.NotLearned);

        Assert.IsTrue(questionProperties4.AddToLearningSession);

        var questionProperties5 = new QuestionProperties();
        questionProperties5 = FilterByKnowledgeSummary_Test(config, _question, questionProperties5, null);

        Assert.IsFalse(questionProperties5.Solid);
        Assert.IsFalse(questionProperties5.NeedsConsolidation);
        Assert.IsFalse(questionProperties5.NeedsLearning);
        Assert.IsTrue(questionProperties5.NotLearned);

        Assert.IsTrue(questionProperties5.AddToLearningSession);
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

        Assert.IsTrue(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsFalse(questionProperties.NotLearned);

        var expectedProbability = _questionValuationSolid.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsTrue(questionProperties.AddToLearningSession);
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

        Assert.IsTrue(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsFalse(questionProperties.NotLearned);

        var expectedProbability = _questionValuationSolid.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsFalse(questionProperties.AddToLearningSession);
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

        Assert.IsFalse(questionProperties.Solid);
        Assert.IsFalse(questionProperties.NeedsConsolidation);
        Assert.IsFalse(questionProperties.NeedsLearning);
        Assert.IsTrue(questionProperties.NotLearned);

        var expectedProbability = _questionValuationNotLearned.CorrectnessProbability;
        Assert.That(questionProperties.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability));
        Assert.IsFalse(questionProperties.AddToLearningSession);

        var questionProperties2 = new QuestionProperties();
        questionProperties2 = FilterByKnowledgeSummary_Test(config, _question, questionProperties2, null);

        Assert.IsFalse(questionProperties2.Solid);
        Assert.IsFalse(questionProperties2.NeedsConsolidation);
        Assert.IsFalse(questionProperties2.NeedsLearning);
        Assert.IsTrue(questionProperties2.NotLearned);

        var expectedProbability2 = _question.CorrectnessProbability;
        Assert.That(questionProperties2.PersonalCorrectnessProbability, Is.EqualTo(expectedProbability2));
        Assert.IsFalse(questionProperties2.AddToLearningSession);
    }
}