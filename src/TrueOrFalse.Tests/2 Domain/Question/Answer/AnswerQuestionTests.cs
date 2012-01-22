using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class AnswerQuestionTests : BaseTest
    {
        [Test]
        public void When_answering_question_answer_history_should_be_created()
        {
            var contextUsers = ContextRegisteredUser.New().Persist();
            var contextQuestion = ContextQuestion.New()
                .AddQuestion("Some Question", "Some answer")
                .AddCategory("A").
                Persist();

            var createdQuestion = contextQuestion.Questions[0];
            var user = contextUsers.Users[0];
            Resolve<AnswerQuestion>().Run(createdQuestion.Id, "Some answer", user.Id);

            var answerHistoryItems= Resolve<AnswerHistoryRepository>().GetAll();
            Assert.That(answerHistoryItems.Count, Is.EqualTo(1));
            Assert.That(answerHistoryItems[0].AnswerText, Is.EqualTo("Some answer"));
            Assert.That(answerHistoryItems[0].UserId, Is.EqualTo(user.Id));
        }
    }
}
