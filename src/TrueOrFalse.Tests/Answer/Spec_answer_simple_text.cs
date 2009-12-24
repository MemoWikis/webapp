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
        ContextQuestion _context = new ContextQuestion();

        public ArrangeQuestion Arrange_question(string questionText)
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
            Arrange_question(" What is BDD ")
                .WithStrictAnswer("Behaviour Driven Development");

            _context.QuestionText.
                IsValidAnswer(new UserInputText("Behaviour Driven Development"))
                    .Should().Be.True(); 
        }

        [Test]
        public void Text_answer_should_be_invalid_on_wrong_input()
        {
            Arrange_question(" What is BDD ")
                .WithStrictAnswer("Invalid answer");

            _context.QuestionText.
                IsValidAnswer(new UserInputText("Behaviour Driven Development"))
                    .Should().Be.False();            
        }
    }
}
