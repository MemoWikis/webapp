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

        public ArrangeQuestion AddAnswer(string answerText, 
                                         string description = "some description",
                                         AnswerType answerType = AnswerType.FreeText)
        {
            Question.Answers.Add(
                new Answer(answerText)
                    {
                        Type = answerType,
                        Description = description
                    }
            );
            return this;
        }
    }
}
