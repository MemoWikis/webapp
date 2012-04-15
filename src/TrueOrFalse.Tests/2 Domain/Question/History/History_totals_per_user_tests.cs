using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests._2_Domain.Question.History
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

            var createdQuestion = contextQuestion.Questions[0];
            var user = contextUsers.Users[0];
            Resolve<AnswerQuestion>().Run(createdQuestion.Id, "Some answer", user.Id);
            
        }
    }
}
