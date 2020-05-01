using System.Collections.Concurrent;
using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests 
{
    class Get_wuwi_from_category : BaseTest
    {
        [Test]
        public void SetWuwi()
        {
            var contextUser = ContextUser.New();
            var users = contextUser.Add().All;
            var userCacheItem = new UserCacheItem();
            userCacheItem.User = users.FirstOrDefault();
            userCacheItem.CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>();
            userCacheItem.SetValuations = new ConcurrentDictionary<int, SetValuation>();
            userCacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuation>();

            var questions = ContextQuestion.New().AddQuestions(20, users.FirstOrDefault(), true).All;
            Sl.UserRepo.Create(users.FirstOrDefault());
            UserCache.AddOrUpdate(users.FirstOrDefault());

           ContextQuestion.PutQuestionValuationsIntoUserCache(questions, users);
        }
    }
}
