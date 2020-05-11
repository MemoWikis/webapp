using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Test_learning_session_steps : BaseTest
    {
        private readonly int LoggedIn = 0;
        private readonly int NotLoggedIn = -1;

        [Test]
        public void Test_answer_should_correct_added_or_not_added_for_logged_in_user()
        {
            var learningSession = ContextLearningSession.GetLearningSessionWithoutAnswerState(LoggedIn, 5);

            learningSession.AddAnswer(new Answer{AnswerredCorrectly = AnswerCorrectness.True});
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

            learningSession.NextStep(); 
            Assert.That(learningSession.CurrentIndex, Is.EqualTo(1));

            learningSession.AddAnswer(new Answer{AnswerredCorrectly = AnswerCorrectness.False});
            Assert.That(learningSession.Steps.Count, Is.EqualTo(6));
            Assert.That(learningSession.Steps.Last().AnswerState, Is.EqualTo(AnswerStateNew.Unanswered));

            learningSession.NextStep();
            Assert.That(learningSession.CurrentIndex, Is.EqualTo(2));

            learningSession.SkipStep();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(7));
            Assert.That(learningSession.Steps.Last().AnswerState, Is.EqualTo(AnswerStateNew.Unanswered));
        }

        [Test]
        public void Test_answer_should_correct_added_or_not_added_for_not_logged_in_user()
        {
            var learningSession = ContextLearningSession.GetLearningSessionWithoutAnswerState(NotLoggedIn, 5);

            learningSession.AddAnswer(new Answer { AnswerredCorrectly = AnswerCorrectness.True });
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

            learningSession.NextStep();
            Assert.That(learningSession.CurrentIndex, Is.EqualTo(1));

            learningSession.AddAnswer(new Answer { AnswerredCorrectly = AnswerCorrectness.False });
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

            learningSession.NextStep();
            Assert.That(learningSession.CurrentIndex, Is.EqualTo(2));
        }
    }
}
