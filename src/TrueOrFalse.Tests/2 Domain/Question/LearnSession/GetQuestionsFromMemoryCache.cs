using System;
using System.Collections.Generic;
using System.Linq;
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
            return LearningSessionNew.GetRandom(amountQuestions, allQuestions, true);
        }

        [Test]
        public void Get_questions_with_low_probabalitiy_of_being_amswered()
        {
            var allQuestions = GetAllQuestionsFromCategory();
            var diffcultQuestions = LearningSessionNew.GetDifficultQuestions(allQuestions);
            
            Assert.That(getCountFromFilteredQuestions(diffcultQuestions, "larger"), Is.EqualTo(0));
        }

        [Test]
        public void Get_questions_with_high_probabalitiy_of_being_amswered()
        {
            var allQuestions = GetAllQuestionsFromCategory();
            var diffcultQuestions = LearningSessionNew.GetSimpleQuestions(allQuestions);

            Assert.That(getCountFromFilteredQuestions(diffcultQuestions, "smaller"), Is.EqualTo(0));
        }

        private List<Question> GetAllQuestionsFromCategory()
        {
            return  ContextQuestion.GetAllQuestionFromMemoryCache(); 
        }

        private int getCountFromFilteredQuestions(List<Question> questions, string isSmallerOrLarger)
        {
            switch (isSmallerOrLarger)
            {
                case "larger":
                    return questions.Where(q => q.CorrectnessProbability < 50).ToList().Count;
                case "smaller":
                    return questions.Where(q => q.CorrectnessProbability > 50).ToList().Count;
                default:
                    throw new NotImplementedException
                        ("Unknown Value");
            }
        }
    }
}
