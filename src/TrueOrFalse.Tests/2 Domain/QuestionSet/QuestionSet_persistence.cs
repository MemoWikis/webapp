using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class QuestionSet_persistence : BaseTest
    {
        [Test]
        public void QuestionSet_should_be_persisted()
        {
            var context = ContextQuestion.New()
                            .AddQuestion("Q1", "A1").AddCategory("A")
                            .AddQuestion("Q2", "A2").AddCategory("A")
                            .AddQuestion("Q3", "A3")
                            .AddQuestion("Q4", "A4").AddCategory("B")
                            .Persist();

            var questionSet = new QuestionSet();
            questionSet.Add(context.Questions);

            Resolve<QuestionSetRepository>().Create(questionSet);

            var questionSetFromDb = Resolve<QuestionSetRepository>().GetAll()[0];
            Assert.That(questionSetFromDb.QuestionsInSet.Count, Is.EqualTo(4));
        }
    }
}
