using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class ArrangeQuestion
    {
        private readonly ContextQuestion _context;
        public readonly Question Question;

        public ArrangeQuestion(ContextQuestion context)
        {
            _context = context;
            Question = new Question();
        }

        public ArrangeQuestion AddAnswer(string answerText, 
                                         string description = "some description",
                                         AnswerType answerType = AnswerType.Exact)
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

        public ArrangeQuestion AddCategory(string categoryName)
        {
            Question.Categories.Add(new Category(categoryName));
            return this;
        }

        public ArrangeQuestion AddQuestion(string questionText)
        {
            return _context.AddQuestion(questionText);
        }

        public void Persist()
        {
            _context.Persist();
        }

    }
}
