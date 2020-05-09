using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BDDish.Model;
using NUnit.Framework;
using Seedworks.Web.State;

namespace TrueOrFalse.Tests
{
    class Test_learning_session_steps : BaseTest
    {
        private readonly int LoggedIn = 0;
        private readonly int NotLoggedIn = -1;

        [Test]
        public void Test_answer_should_correct_added_or_not_added()
        {
            var learningSession = GetLearningSession(LoggedIn, 5);

            learningSession.SetAnswer(new Answer{AnswerredCorrectly = AnswerCorrectness.True});
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

            learningSession.NextStep(); 
            Assert.That(learningSession.CurrentIndex, Is.EqualTo(1));

            learningSession.SetAnswer(new Answer{AnswerredCorrectly = AnswerCorrectness.False});
            Assert.That(learningSession.Steps.Count, Is.EqualTo(6));
            Assert.That(learningSession.Steps.Last().AnswerState, Is.EqualTo(AnswerStateNew.Unanswered));

            learningSession.NextStep();
            Assert.That(learningSession.CurrentIndex, Is.EqualTo(2));

            learningSession.SkipStep();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(7));
            Assert.That(learningSession.Steps.Last().AnswerState, Is.EqualTo(AnswerStateNew.Unanswered));
        }


        private LearningSessionNew GetLearningSession(int userId, int amountQuestions = 20)
        {
            var learningSession = GetLearningSessionWithoutAnswerState(userId, amountQuestions);

            return learningSession; 
        }

        private LearningSessionNew GetLearningSessionWithoutAnswerState(int userId, int amountQuestions)
        {
            ContextQuestion.PutQuestionsIntoMemoryCache(amountQuestions);
            var steps = ContextLearningSession.GetSteps(amountQuestions);
            return new LearningSessionNew(steps.ToList(), new LearningSessionConfig { UserId = userId });
        }
    }
}
