using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Sort_by_probability : BaseTest
    {
        [Test]
        public void Should_be_sorted()
        {
            ContextQuestion.PutQuestionIntoMemoryCache(answerProbability: 50, 0); 
            ContextQuestion.PutQuestionIntoMemoryCache(answerProbability: 10,1);
            ContextQuestion.PutQuestionIntoMemoryCache(answerProbability: 20, 2);

            var learningSession = LearningSessionCreator.ForLoggedInUser(new LearningSessionConfig {CategoryId = 0, AllQuestions = true});
            
            //Todo: GetLearningSessionForAnonymusUser user probability
            Assert.That(learningSession.Steps[0].Question.CorrectnessProbability, Is.EqualTo(50)) ;
            Assert.That(learningSession.Steps[1].Question.CorrectnessProbability, Is.EqualTo(20));
            Assert.That(learningSession.Steps[2].Question.CorrectnessProbability, Is.EqualTo(10));
        }
    }
}
