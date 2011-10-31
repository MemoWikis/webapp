using System;
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
        readonly ContextQuestion _context = new ContextQuestion();

        [Test]
        public void Question_should_be_persisted()
        {
            _context.Arrange_question("What is BDD")
                .AddAnswer("Behaviour Driven Development")
                .AddAnswer("Another name for writing acceptance tests");

            var questionService = Resolve<QuestionRepository>();
            questionService.Create(_context.Question);

            var questions = questionService.GetAll();
            questions.Count.Should().Be.EqualTo(1);
            questions[0].Answers.Count.Should().Be.EqualTo(2);
            questions[0].Answers[0].Text.StartsWith("Behaviour").Should().Be.True();
            questions[0].Answers[0].Type = AnswerType.FreeText;
            questions[0].Answers[1].Text.StartsWith("Another").Should().Be.True();
        }

    }
}
