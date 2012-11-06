using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class QuestionAnswerHistoryPersistenceTests : BaseTest
    {
        [Test]
        public void Persistence_Test()
        {
            var questionHistory = new AnswerHistory();
            questionHistory.QuestionId = 12;
            questionHistory.Milliseconds = 100;
            questionHistory.UserId = 1;
            questionHistory.AnswerText = "asdfasfsf";

            Resolve<AnswerHistoryRepository>().Create(questionHistory);
        }
    }
}
