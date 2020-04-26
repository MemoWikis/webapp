using System.Collections.Generic;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    
    class GetQuestionsFromMemoryCache: BaseTest
    {
        [Test]
        public void HaveAllQuestions()
        {
            var allQuestions = ContextQuestion.GetAllQuestionFromMemoryCache(); 
            Assert.That(allQuestions.Count, Is.GreaterThan(4999));
        }

        [Test]
        public void Get_an_amount_of_random_questions()
        {
            Assert.That(Get_an_amount_of_random_questions(4000).Count, Is.EqualTo(4000));
            Assert.That(Get_an_amount_of_random_questions(0).Count, Is.EqualTo(0));
            Assert.That(Get_an_amount_of_random_questions(6000).Count, Is.EqualTo(0));
        }

        private List<Question> Get_an_amount_of_random_questions(int amountQuestions)
        {
            var allQuestions = ContextQuestion.GetAllQuestionFromMemoryCache();
            return LearningSessionNew.GetRandom(amountQuestions, allQuestions); 
        }
    }
}
