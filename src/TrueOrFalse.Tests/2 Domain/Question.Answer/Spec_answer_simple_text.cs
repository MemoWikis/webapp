using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    [Category(TestCategories.BehaviourTest)]
    public class Spec_answer_simple_text
    {
        [Test]
        public void Text_answer_should_be_equal_to_valid_user_input()
        {
            ContextQuestion.New().AddQuestion("What is BDD")
                .AddAnswer("Behaviour Driven Development");

            //_context.Question.
            //    IsValidAnswer(new UserInputText("Behaviour Driven Development"))
            //        .Should().Be.True();
        }

        [Test]
        public void Text_answer_should_be_agnostic_to_white_spaces()
        {
            ContextQuestion.New().AddQuestion(" What is BDD ")
                .AddAnswer("Behaviour Driven Development");

            //_context.Question.
            //    IsValidAnswer(new UserInputText("Behaviour Driven Development"))
            //        .Should().Be.True(); 
        }

        [Test]
        public void Text_answer_should_be_invalid_on_wrong_input()
        {
            ContextQuestion.New().AddQuestion(" What is BDD ")
                .AddAnswer("Invalid answer");

            //_context.Question.
            //    IsValidAnswer(new UserInputText("Behaviour Driven Development"))
            //        .Should().Be.False();
        }
    }
}
