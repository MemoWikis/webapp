using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Sort_diffculitiy_first : BaseTest
    {
        [Test]
        public void DiffculityFirst()
        {
            var categoryId = 0; 
            ContextQuestion.PutQuestionsIntoMemoryCache(20);
            var questions = GetQuestionFromMemoryCacheById(categoryId).
                OrderByDescending(q => q.CorrectnessProbability).ToList();
            var questionsFromLearningSession =
                LearningSessionNewCreator.ForLoggedInUser(new LearningSessionConfig() {CategoryId = 0});
            Assert.That(questions.FirstOrDefault().Id, Is.EqualTo(questionsFromLearningSession.Steps.FirstOrDefault().Question.Id)) ;
        }

        private IList<Question> GetQuestionFromMemoryCacheById(int categoryId)
        {
            return EntityCache.GetQuestionsForCategory(categoryId);
        }

    }
}
