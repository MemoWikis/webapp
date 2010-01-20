using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Answer
{
    [TestFixture]
    public class Spec_answer_persistance
    {
        readonly ContextQuestion _context = new ContextQuestion();

        [Test]
        public void Answer_should_be_persisted()
        {
            _context.Arrange_question("What is BDD")
                .WithStrictAnswer("Behaviour Driven Development");            

            var questionService = new QuestionService();
            questionService.Create(_context.Question);
            
        }
    }
}
