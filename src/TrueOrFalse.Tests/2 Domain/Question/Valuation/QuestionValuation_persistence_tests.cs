﻿using System.Collections.Generic;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class QestionValuation_persistence_tests : BaseTest
    {
        [Test]
        public void QuestionValuation_should_be_persisted()
        {
            var contextQuestion = ContextQuestion.New().AddQuestion("a", "b").Persist();
            var questionValuation = 
                new QuestionValuation  
                {
                    Question = contextQuestion.All[0],
                    User = contextQuestion.Creator,
                    Quality = 91,
                    RelevanceForAll = 40,
                    RelevancePersonal = 7
                };

            Resolve<QuestionValuationRepo>().Create(questionValuation);
        }

        [Test]
        public void Should_select_by_question_ids()
        {
            var context = ContextQuestion.New()
                .AddQuestion("1", "a")
                .AddQuestion("2", "a")
                .AddQuestion("3", "a")
                .AddQuestion("4", "a")
                .AddQuestion("5", "a")
                .Persist();

            var contextUsers = ContextUser.New()
                .Add("User1")
                .Add("User2")
                .Persist();

            var user1 = contextUsers.All[0];
            var user2 = contextUsers.All[1];

            var questionValuation1 = new QuestionValuation { Question = context.All[0], User = user1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation2 = new QuestionValuation { Question = context.All[1], User = user1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation3 = new QuestionValuation { Question = context.All[2], User = user2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation4 = new QuestionValuation { Question = context.All[3], User = user1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation5 = new QuestionValuation { Question = context.All[4], User = user2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };

            Resolve<QuestionValuationRepo>().Create(
                new List<QuestionValuation> { questionValuation1, questionValuation2, questionValuation3, questionValuation4, questionValuation5});

            Assert.That(Resolve<QuestionValuationRepo>().GetActiveInWishknowledge(
                new[] { context.All[0].Id, context.All[1].Id, context.All[2].Id }, user1.Id).Count, Is.EqualTo(2));
        }
    }
}
