﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using SharpTestsEx;
using TrueOrFalse.Core;


namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Question_persistence_tests : BaseTest
    {
        [Test]
        public void Questions_should_be_persisted()
        {
            ContextQuestion.New().AddQuestion("What is BDD")
                                    .AddAnswer("Behaviour Driven Development")
                                    .AddAnswer("Another name for writing acceptance tests")
                                    .AddCategory("A")
                                    .AddCategory("B")
                                    .AddCategory("C")
                                 .AddQuestion("Another Question")
                                    .AddAnswer("Some answer")
                                 .Persist();

            var questions = Resolve<QuestionRepository>().GetAll();
            questions.Count.Should().Be.EqualTo(2);
            questions[0].Categories.Count.Should().Be.EqualTo(3);
            questions[0].Categories[0].Name.Should().Be.EqualTo("A");
            questions[0].Categories[2].Name.Should().Be.EqualTo("C");
            questions[0].Answers.Count.Should().Be.EqualTo(2);
            questions[0].Answers[0].Text.StartsWith("Behaviour").Should().Be.True();
            questions[0].Answers[1].Text.StartsWith("Another").Should().Be.True();
        }

    }
}
