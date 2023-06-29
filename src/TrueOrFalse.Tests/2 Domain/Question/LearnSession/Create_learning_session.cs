using Autofac;
using NUnit.Framework;
using TrueOrFalse.Tests;

class Create_learning_session : BaseTest
{
    [Test]
    public void GetCorrectProbabilityQuestions()
    {
        ContextQuestion.PutQuestionsIntoMemoryCache( LifetimeScope.Resolve<CategoryRepository>());
        var learningSession = ContextLearningSession.GetLearningSession(
            new LearningSessionConfig
            {
                MaxQuestionCount = 5,
                CategoryId = 1
            });

        foreach (var step in learningSession.Steps)
        {
            Assert.That(step.Question.CorrectnessProbability, Is.LessThan(51));
            Assert.That(step.Question.CorrectnessProbability, Is.GreaterThan(9));
        }
    }
}