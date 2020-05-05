using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Sort_by_probability : BaseTest
    {
        [Test]
        public void Should_be_sorted()
        {
            ContextQuestion.PutQuestionIntoMemoryCache(answerProbability: 50);
            ContextQuestion.PutQuestionIntoMemoryCache(answerProbability: 10);
            ContextQuestion.PutQuestionIntoMemoryCache(answerProbability: 20);

            var learningSession = LearningSessionNewCreator.ForLoggedInUser(new LearningSessionConfig {CategoryId = 0});
            
            //Todo: Get user probability
            Assert.That(learningSession.Steps[0].Question.CorrectnessProbability, Is.EqualTo(10)) ;
            Assert.That(learningSession.Steps[1].Question.CorrectnessProbability, Is.EqualTo(20));
            Assert.That(learningSession.Steps[2].Question.CorrectnessProbability, Is.EqualTo(30));
        }
    }
}
