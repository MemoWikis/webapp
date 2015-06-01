using System;
using NUnit.Framework;

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

            Assert.That(answerStatsInPeriond[0].TotalAnswers, Is.EqualTo(3));
            Assert.That(answerStatsInPeriond[0].TotalFalseAnswers, Is.EqualTo(2));
            Assert.That(answerStatsInPeriond[0].TotalTrueAnswers, Is.EqualTo(1));
        }
    }
}
