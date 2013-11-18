using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class Should_retrieve_stats_in_time_period : BaseTest
    {
        [Test]
        public void Run()
        {
            var contextUsers = ContextRegisteredUser.New().Add().Persist();
            var contextQuestion = ContextQuestion.New()
                    .AddQuestion("Question", "Answer")
                    .AddCategory("A").
                Persist();

            var createdQuestion = contextQuestion.All[0];
            var user = contextUsers.Users[0];

            Resolve<AnswerQuestion>().Run(createdQuestion.Id, "Answer", user.Id);
            Resolve<AnswerQuestion>().Run(createdQuestion.Id, "...,", user.Id);
            Resolve<AnswerQuestion>().Run(createdQuestion.Id, "...,", user.Id);

            var answerStatsInPeriond = 
                Resolve<GetAnswerStatsInPeriod>().Run(user.Id, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            Assert.That(answerStatsInPeriond.TotalAnswers, Is.EqualTo(3));
            Assert.That(answerStatsInPeriond.TotalFalseAnswers, Is.EqualTo(2));
            Assert.That(answerStatsInPeriond.TotalTrueAnswers, Is.EqualTo(1));
        }
    }
}
