﻿using System.Linq;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class Should_calculate_wish_knowledge_count : BaseTest
    {
        [Test]
        public void Run()
        {
            var contextQuestion = ContextQuestion.New()
                .AddQuestion("QuestionA", "AnswerA").AddCategory("A")
                .Persist();

            var questionId = contextQuestion.All.First().Id;
            var updateTotals = Resolve<UpdateQuestionTotals>();

            updateTotals.Run(new QuestionValuation { RelevancePersonal = 100, QuestionId = questionId, UserId = 2 });
            updateTotals.Run(new QuestionValuation { RelevancePersonal = 1, QuestionId = questionId, UserId = 2 });
            updateTotals.Run(new QuestionValuation { QuestionId = questionId, UserId = 2 });

            Assert.That(Resolve<GetWishQuestionCount>().Run(userId:2), Is.EqualTo(2));
        }
    }
}
