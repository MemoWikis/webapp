using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class Spec_changing_question_categories : BaseTest
    {
        [Test]
        public void Should_change_categories()
        {
            var questionContext =
                ContextQuestion.New().AddQuestion(questionText: "Question", solutionText: "Answer")
                                       .AddCategory("A")
                                       .AddCategory("B")
                                       .AddCategory("C")
                                     .Persist();

            var question = questionContext.All[0];
            var categoryA = question.Categories.ByName("A");
            var categoryB = question.Categories.ByName("B");

            question.Categories.Clear();
            question.Categories.Add(categoryA);
            question.Categories.Add(categoryB);

            var categoryRepository = Resolve<QuestionRepo>();
            categoryRepository.Update(question);

            var questionFromDb = categoryRepository.GetAll()[0];
            Assert.That(questionFromDb.Categories.Count, Is.EqualTo(2));
            Assert.That(questionFromDb.Categories[0].Name, Is.EqualTo("A"));
            Assert.That(questionFromDb.Categories[1].Name, Is.EqualTo("B"));
        }
    }
}