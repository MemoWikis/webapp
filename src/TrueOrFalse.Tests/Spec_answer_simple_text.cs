using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class Spec_answer_simple_text
    {
        Context _context = new Context();

        public class Context
        {
            public Quiz Quiz;
            public User User;
            public QuestionText QuestionText;
            public AnswerText AnswerText;
        }

        public ArrangeQuestion Arrange_question(string s)
        {
            var result = new ArrangeQuestion();
            _context.QuestionText = result.QuestionText;
            return result;
        }


        [Test]
        public void Text_answer_should_be_equal_to_valid_user_input()
        {
            Arrange_question("What is BDD")
                .WithStrictAnswer("Behaviour Driven Development");

            _context.QuestionText.
                IsValidAnswer(new UserInputText("Behaviour Driven Development"))
                    .Should().Be.True();

        }

        [Test]
        public void Text_answer_should_be_agnostic_to_white_spaces()
        {
            
        }
    }
}
