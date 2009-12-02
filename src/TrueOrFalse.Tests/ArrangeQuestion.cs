using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class ArrangeQuestion
    {
        public QuestionText QuestionText;

        public ArrangeQuestion()
        {
            QuestionText = new QuestionText();
        }

        public void WithStrictAnswer(string answerText)
        {
            QuestionText.AnswerText = new AnswerText();
            QuestionText.AnswerText.Text = answerText;
        }
    }
}
