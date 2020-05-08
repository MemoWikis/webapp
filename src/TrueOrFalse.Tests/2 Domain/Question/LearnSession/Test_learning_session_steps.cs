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
        private readonly int LoggedIn = 0;
        private readonly int NotLoggedIn = -1;

        [Test]
        public void Should_by_skip_or_false_answer_addet()
        {
            Assert.That(Get_learning_session(LoggedIn,"falseAnswered").Steps.Count, Is.EqualTo(21)); 
            Assert.That(Get_learning_session(LoggedIn,"skipAnswered").Steps.Count, Is.EqualTo(20));  
        }

        [Test]
        public void Should_by_skip_or_false_not_addet()
        {
            Assert.That(Get_learning_session(NotLoggedIn, "falseAnswered").Steps.Count, Is.EqualTo(20)); //not LoggedIn
            Assert.That(Get_learning_session(NotLoggedIn, "skipAnswered").Steps.Count, Is.EqualTo(20)); //not LoggedIn
        }

        [Test]
        public void should_by_correct_answer_not_addet()
        {
            Assert.That(Get_learning_session(LoggedIn, "correctAnswered").Steps.Count, Is.EqualTo(20)); //LoggedIn
            Assert.That(Get_learning_session(NotLoggedIn, "correctAnswered").Steps.Count, Is.EqualTo(20)); //not LoggedIn
        }

        private  LearningSessionNew Get_learning_session(int userId, string answerState, int amountQuestions = 20)
        {
            ContextQuestion.PutQuestionsIntoMemoryCache(amountQuestions);
            var steps = new Get_questions_from_memory_cache().Get_steps(20);
            
            var learningSession = new LearningSessionNew(steps.ToList(), new LearningSessionConfig{UserId = userId}, 0);

            switch (answerState)
            {
                case "falseAnswered" :
                    learningSession.SetFalseAnswer(0);
                    break;
                case "correctAnswered":
                    learningSession.SetCorrectAnswer(0);
                    break;
                case "skipAnswered": 
                    learningSession.SetSkipAnswer(0);
                    break;
                default:
                    throw new Exception("unknown answerstate"); 
            }

            return learningSession; 
        }
    }
}
