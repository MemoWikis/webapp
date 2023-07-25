using Autofac;
using NUnit.Framework;
using TrueOrFalse.Tests;

class Create_learning_session : BaseTest
{
    [Test]
    public void GetCorrectProbabilityQuestions()
    {
        ContextQuestion.PutQuestionsIntoMemoryCache( R<CategoryRepository>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            R<UserRepo>(), 
            R<QuestionWritingRepo>());
        var learningSessionConfig = new LearningSessionConfig
        {
            MaxQuestionCount = 5,
            CategoryId = 1
        };
        var learningSession = new ContextLearningSession(R<CategoryRepository>(),
            R<LearningSessionCreator>(), 
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            learningSessionConfig, 
            R<UserRepo>(), 
            R<QuestionWritingRepo>())
            .GetLearningSession();

        foreach (var step in learningSession.Steps)
        {
            Assert.That(step.Question.CorrectnessProbability, Is.LessThan(51));
            Assert.That(step.Question.CorrectnessProbability, Is.GreaterThan(9));
        }
    }
}