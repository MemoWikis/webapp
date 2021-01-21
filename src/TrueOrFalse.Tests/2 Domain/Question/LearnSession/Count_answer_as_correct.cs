using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests._2_Domain.Question.LearnSession
{
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

            var learningSession = ContextLearningSession.GetLearningSessionWithUser(1, 5);
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(4));

            learningSession = ContextLearningSession.GetLearningSession(
                new LearningSessionConfig
                {
                    CurrentUserId = 1,
                    IsInTestMode = true,
                    MaxQuestionCount = 5, 
                    CategoryId = 1,
                    AllQuestions = true
                });
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));

        }

        [Test]
        public void SetAnswerAsCorrectTestModeAndWishSession()
        {
           var lastUserCashItem =  ContextQuestion.SetWuwi(10).Last();
            var learningSession = ContextLearningSession.GetLearningSession(
                new LearningSessionConfig
                {
                    CurrentUserId = lastUserCashItem.UserId,
                    IsInTestMode = true,
                    InWishknowledge = true,
                    MaxQuestionCount = 5, 
                    CategoryId = 1

                });

            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(5));
        }

        [Test]
        public void SetAnswerAsCorrectWishSession(){
            var lastUserCashItem = ContextQuestion.SetWuwi(10).Last();
            var learningSession = ContextLearningSession.GetLearningSession(
                new LearningSessionConfig
                {
                    CurrentUserId = lastUserCashItem.UserId,
                    IsInTestMode = false,
                    InWishknowledge = true,
                    MaxQuestionCount = 5,
                    CategoryId = 1
                });
            learningSession.SetCurrentStepAsCorrect();
            Assert.That(learningSession.Steps.Count, Is.EqualTo(4));
        }
    }
}
