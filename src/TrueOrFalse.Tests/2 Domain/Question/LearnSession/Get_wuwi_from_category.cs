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
            ContextQuestion.PutQuestionsIntoMemoryCache();
            ContextQuestion.SetWuwi(20);
            
            var categoryId = 1;
            var usercashItem = UserCache.GetAllCacheItems().Where(uci => uci.User.Name == "Daniel" ).First();

            var wuwis = usercashItem.QuestionValuations
                .Select(qv => qv.Value)
                .Where(qv=> qv.IsInWishKnowledge && qv.Question.CategoriesIds.Any(c=> c.Id == categoryId) )
                .ToList();

            var wuwisFromLearningSession = LearningSessionCreator.ForLoggedInUser(new LearningSessionConfig
                {InWishknowledge = true, CategoryId = categoryId, CurrentUserId = usercashItem.UserId});

            Assert.That(wuwisFromLearningSession.Steps.Count, Is.EqualTo(wuwis.Count));
        }
    }
}

