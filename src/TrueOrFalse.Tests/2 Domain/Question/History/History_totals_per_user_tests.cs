using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class History_totals_per_user_tests : BaseTest
    {
        [Test]
        public void Run()
        {
            var contextUsers = ContextRegisteredUser.New().Add().Add().Persist();
            var contextQuestion = ContextQuestion.New()
                .AddQuestion("QuestionA", "AnswerA").AddCategory("A")
                .AddQuestion("QuestionB", "QuestionB").AddCategory("A").
                Persist();

            var createdQuestion1 = contextQuestion.Questions[0];
            var createdQuestion2 = contextQuestion.Questions[1];
            var user1 = contextUsers.Users[0];
            
            Resolve<AnswerQuestion>().Run(createdQuestion1.Id, "Some answer 1", user1.Id);
            Resolve<AnswerQuestion>().Run(createdQuestion1.Id, "Some answer 2", user1.Id);
            Resolve<AnswerQuestion>().Run(createdQuestion1.Id, "AnswerA", user1.Id);

            var totalsPerUserLoader = Resolve<TotalsPersUserLoader>();
            var totalsResult = totalsPerUserLoader.Run(user1.Id, new[] { createdQuestion1.Id, createdQuestion2.Id});

            Assert.That(totalsResult.ByQuestionId(createdQuestion1.Id).TotalFalse, Is.EqualTo(2));
            Assert.That(totalsResult.ByQuestionId(createdQuestion1.Id).TotalTrue, Is.EqualTo(1));
        }
    }
}
