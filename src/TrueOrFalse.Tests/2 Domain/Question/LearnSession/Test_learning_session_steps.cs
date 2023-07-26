using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

class Test_learning_session_steps : BaseTest
{
    private readonly int UserId = 1;

    [Test]
    public void Test_answer_should_correct_added_or_not_added_for_logged_in_user()
    {
        var learningSessionConfig = new LearningSessionConfig
        {
            CurrentUserId = UserId,
            MaxQuestionCount = 5,
            CategoryId = 1,
            Repetition = RepetitionType.Normal
        }; 
        var learningSession = new ContextLearningSession(R<CategoryRepository>(), 
            R<LearningSessionCreator>(), 
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            learningSessionConfig,
            R<UserReadingRepo>(),
            R<QuestionWritingRepo>())
            .GetLearningSessionWithUser();

        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = true });
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

        learningSession.NextStep();
        Assert.That(learningSession.CurrentIndex, Is.EqualTo(1));

        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = false });
        Assert.That(learningSession.Steps.Count, Is.EqualTo(6));
        Assert.That(learningSession.Steps.Last().AnswerState, Is.EqualTo(AnswerState.Unanswered));

        learningSession.NextStep();
        Assert.That(learningSession.CurrentIndex, Is.EqualTo(2));

        learningSession.SkipStep();
        var currentStep = learningSession.CurrentIndex - 1;
        Assert.That(learningSession.Steps.Count, Is.EqualTo(6));
        Assert.That(learningSession.Steps[currentStep].AnswerState, Is.EqualTo(AnswerState.Skipped));
    }

    [Test]
    public void Test_answer_should_correct_added_or_not_added_for_not_logged_in_user()
    {
        var learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            new LearningSessionConfig(),
            R<UserReadingRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSessionForAnonymusUser(5);

        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = true });
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

        learningSession.NextStep();
        Assert.That(learningSession.CurrentIndex, Is.EqualTo(1));

        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = false });
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

        learningSession.NextStep();
        Assert.That(learningSession.CurrentIndex, Is.EqualTo(2));
    }

    [Test]
    public void Test_is_last_step()
    {
        var catRepo = R<CategoryRepository>();
        var leraningSessionCreator = R<LearningSessionCreator>(); 
        var learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            new LearningSessionConfig(),
            R<UserReadingRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSessionForAnonymusUser(1);
        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = true });
        learningSession.NextStep();
        Assert.That(learningSession.IsLastStep, Is.EqualTo(true));

        learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(), 
            R<AnswerRepo>(), 
            R<AnswerQuestion>(), 
            new LearningSessionConfig(), 
            R<UserReadingRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSessionForAnonymusUser(1);

        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = false });
        learningSession.NextStep();
        Assert.That(learningSession.IsLastStep, Is.EqualTo(true));

        var learningsessionConfig = new LearningSessionConfig
        {
            CurrentUserId = UserId,
            MaxQuestionCount = 1,
            CategoryId = 1,
            Repetition = RepetitionType.None
        }; 

        learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            learningsessionConfig,
            R<UserReadingRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSessionWithUser();

        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = false });
        learningSession.NextStep();
        Assert.That(learningSession.IsLastStep, Is.EqualTo(false));

        learningSession.AddAnswer(new AnswerQuestionResult { IsCorrect = true });
        learningSession.NextStep();
        Assert.That(learningSession.IsLastStep, Is.EqualTo(true));
    }
}