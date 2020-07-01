using System.Collections.Concurrent;
using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Get_wuwi_from_category : BaseTest
    {
        [Test]
        public void GetWuwiSession()
        {
            ContextQuestion.SetWuwi(20);
            var categoryId = 0;
            var usercashItem = UserCache.GetAllCacheItems().Last();
            
            var wuwis = usercashItem.QuestionValuations
                .Select(qv => qv.Value)
                .Where(qv=> qv.IsInWishKnowledge() && qv.Question.Categories.Any(c=> c.Id == categoryId) )
                .ToList();

            var wuwisFromLearningSession = LearningSessionNewCreator.ForLoggedInUser(new LearningSessionConfig
                {IsWishSession = true, CategoryId = categoryId, UserId = usercashItem.UserId});

            Assert.That(wuwisFromLearningSession.Steps.Count, Is.EqualTo(wuwis.Count));
        }
    }
}

