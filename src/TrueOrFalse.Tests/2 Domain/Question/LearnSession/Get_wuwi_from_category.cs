using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

            PutQuestionValuationsIntoUserCache(questions, users);

            var c = UserCache.GetAllCacheItems();

        }

        private static void PutQuestionValuationsIntoUserCache(List<Question> questions, List<User> users)
        {
            var rand = new Random();
            for (int i = 0; i < questions.Count; i++)
            {
                var questionValuation = new QuestionValuation();

                questionValuation.Id = i;
                questionValuation.Question = questions[i];

                if (i == 0)
                    questionValuation.RelevancePersonal = -1;
                else
                    questionValuation.RelevancePersonal = rand.Next(-1, 2);

                questionValuation.User = users.FirstOrDefault();
                UserCache.AddOrUpdate(questionValuation);
            }
        }
    }
}
