using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using TrueOrFalse.Tests.Answer;

namespace TrueOrFalse.Tests.Persistence
{
	[TestFixture]
    [Category(TestCategories.Programmer)]
	public class Persistence_Tests
	{
        readonly ContextQuestion _context = new ContextQuestion();

        [Test]
        public void Answer_should_be_persisted()
        {
            _context.Arrange_question("What is BDD")
                .WithStrictAnswer("Behaviour Driven Development");

            var questionService = new QuestionService();
            questionService.Create(_context.Question);

        	questionService.GetAll()
				.Count.Should().Be.EqualTo(1);
        }

	}
}
