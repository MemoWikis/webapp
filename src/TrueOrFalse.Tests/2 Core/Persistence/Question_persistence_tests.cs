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
                .WithAnswer("Behaviour Driven Development");

            var questionService = Resolve<QuestionRepository>();
            questionService.Create(_context.Question);

            questionService.GetAll()
                .Count.Should().Be.EqualTo(1);
        }

    }
}
