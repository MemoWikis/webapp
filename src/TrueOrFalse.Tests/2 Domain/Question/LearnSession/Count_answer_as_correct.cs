using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

class Count_answer_as_correct : BaseTest
{
    [Test]
    public void SetAnswerAsCorrectAnonymus()
    {
        var learningSession = ContextLearningSession.GetLearningSessionForAnonymusUser(5);
        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.CurrentStep.AnswerState, Is.EqualTo(AnswerState.Correct));
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));
    }

    [Test]
    public void SetAnswerAsCorrectLoggedIn()
    {
        var learningSession = ContextLearningSession.GetLearningSessionWithUser(new LearningSessionConfig
        {
            CurrentUserId = 1,
            MaxQuestionCount = 5,
            CategoryId = 1
        });
        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(4));

        learningSession = ContextLearningSession.GetLearningSession(
            new LearningSessionConfig
            {
                CurrentUserId = 1,
                IsInTestMode = true,
                MaxQuestionCount = 5, 
                CategoryId = 1,
            });

        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));
    }

    [Test]
    public void SetAnswerAsCorrectTestModeAndWishSession()
    {
        var lastUserCacheItem =  ContextQuestion.SetWuwi(1000).Last();
        var learningSession = ContextLearningSession.GetLearningSession(
            new LearningSessionConfig
            {
                CurrentUserId = lastUserCacheItem.Id,
                IsInTestMode = true,
                InWuwi = true,
                MaxQuestionCount = 5, 
                CategoryId = 1
            });

        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(5));
    }

    [Test]
    public void SetAnswerAsCorrectWishSession(){
        var lastUserCacheItem = ContextQuestion.SetWuwi(10).Last();
        var learningSession = ContextLearningSession.GetLearningSession(
            new LearningSessionConfig
            {
                CurrentUserId = lastUserCacheItem.Id,
                IsInTestMode = false,
                InWuwi = true,
                MaxQuestionCount = 5,
                CategoryId = 1
            });
        learningSession.SetCurrentStepAsCorrect();
        Assert.That(learningSession.Steps.Count, Is.EqualTo(4));
    }
}