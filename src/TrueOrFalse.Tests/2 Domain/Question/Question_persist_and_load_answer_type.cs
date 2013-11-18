using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class Question_persist_and_load_answer_type : BaseTest
    {
        [Test]
        public void Should_store_answer_type()
        {
            var context = ContextQuestion.New().AddQuestion("What is BDD", "Another name for writing acceptance tests")
                                    .AddCategory("A")
                                    .AddCategory("B")
                                    .AddCategory("C")
                                 .AddQuestion("Another Question", "Some answer")
                                    .Persist();

            var question = context.All[0];
        }
    }
}
