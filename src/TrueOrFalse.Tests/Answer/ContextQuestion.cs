using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class ContextQuestion
    {
        public User User;
        public Question Question;
        public Core.Answer Answer;

        public ArrangeQuestion Arrange_question(string questionText)
        {
            var result = new ArrangeQuestion();
            Question = result.Question;
            return result;
        }
    }
}
