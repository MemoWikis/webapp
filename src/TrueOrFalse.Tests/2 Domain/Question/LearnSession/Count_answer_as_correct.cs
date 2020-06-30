using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests._2_Domain.Question.LearnSession
{
    class Count_answer_as_correct : BaseTest
    {

        [Test]
        public void SetAnswerAsCorrect()
        {
            var learningSession = ContextLearningSession.GetLearningSessionForAnonymusUser(5);
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.CurrentStep.AnswerState, Is.EqualTo(AnswerStateNew.Correct));
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

            learningSession = ContextLearningSession.GetLearningSessionWithUser(1, 5);
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(4));

            learningSession = ContextLearningSession.GetLearningSession(
                new LearningSessionConfig
                {
                    UserId = 1,
                    IsInTestMode = true,
                    MaxQuestions = 5
                });
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

            learningSession = ContextLearningSession.GetLearningSession(
                new LearningSessionConfig
                {
                    UserId = 1,
                    IsInTestMode = true,
                    IsWishSession = true,
                    MaxQuestions = 5

                });
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

            learningSession = ContextLearningSession.GetLearningSession(
                new LearningSessionConfig
                {
                    UserId = 1,
                    IsInTestMode = false,
                    IsWishSession = true,
                    MaxQuestions = 5

                });
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(4));
        }
    }
}
