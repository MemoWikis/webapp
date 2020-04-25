using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpTestsEx.ExtensionsImpl;

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

       // [TestCase(4000)]
       [Test]
        public void Test_Get_a_certain_number_of_random_questions()
        {
            Assert.That(Get_a_certain_number_of_random_questions(4000).Count, Is.EqualTo(4000));
            Assert.That(Get_a_certain_number_of_random_questions(0).Count, Is.EqualTo(0));
            Assert.That(Get_a_certain_number_of_random_questions(6000).Count, Is.EqualTo(0));
        }

        private List<Question> Get_a_certain_number_of_random_questions(int amountQuestions)
        {
            var allQuestions = ContextQuestion.GetAllQuestionFromMemoryCache();
            var rand = new Random();
            var randomQuestionsList = new List<Question>();

            if (amountQuestions > allQuestions.Count || amountQuestions < 2)
                return new List<Question>();

            for (var i = 0; i < amountQuestions; i++)
            {
                var nextQuestion = rand.Next(0, allQuestions.Count-1);
                allQuestions.RemoveAt(nextQuestion);
                randomQuestionsList.Add(allQuestions[nextQuestion]);
               
            }
            return randomQuestionsList; 
        }
    }
}
