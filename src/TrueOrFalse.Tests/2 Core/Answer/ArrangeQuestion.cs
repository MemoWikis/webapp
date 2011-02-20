using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class ArrangeQuestion
    {
        public Question Question;

        public ArrangeQuestion()
        {
            Question = new Question();
        }

        public void With(string questionText)
        {
            
        }

        public void WithStrictAnswer(string answerText)
        {
            Question.Answer = new Answer();
            Question.Answer.Text = answerText;
        }
    }
}
