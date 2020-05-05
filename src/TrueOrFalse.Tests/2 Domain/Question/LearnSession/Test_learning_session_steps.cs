using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDDish.Model;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Test_learning_session_steps : BaseTest
    {
        [Test]
        public void Should_by__skip_or_false_answer_addet()
        {
            ContextQuestion.PutQuestionsIntoMemoryCache(20);
            var learningSession =
                LearningSessionNewCreator.ForLoggedInUser(new LearningSessionConfig {MaxQuestions = 20});

            var stepping = new Stepping(learningSession.Steps.ToList());
            stepping.SetAnswerLearningSessionStepAnswerNew(0, true, true, false); //correct answer
            Assert.That(stepping.LearningSessionSteps.Count, Is.EqualTo(20));
            
            stepping.SetAnswerLearningSessionStepAnswerNew(1, false, true, false);   // incorrect answer
            Assert.That(stepping.LearningSessionSteps.Count, Is.EqualTo(21));

            stepping.SetAnswerLearningSessionStepAnswerNew(2, false, false, false); // not answer
            Assert.That(stepping.LearningSessionSteps.Count, Is.EqualTo(21));

            stepping.SetAnswerLearningSessionStepAnswerNew(2, false, false, true); // skip
            Assert.That(stepping.LearningSessionSteps.Count, Is.EqualTo(22));
        }
    }
}
