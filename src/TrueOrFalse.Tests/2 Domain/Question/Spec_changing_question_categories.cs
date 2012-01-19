using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class Spec_changing_question_categories : BaseTest
    {
        [Test]
        public void Should_change_categories()
        {
            var questionContext =
                ContextQuestion.New().AddQuestion("Question", "Answer")
                                       .AddCategory("A")
                                       .AddCategory("B")
                                       .AddCategory("C")
                                     .Persist();

            var question = questionContext.Questions[0];
            var categoryA = question.Categories.ByName("A");
            var categoryB = question.Categories.ByName("B");

            question.Categories.Clear();
            question.Categories.Add(categoryA);
            question.Categories.Add(categoryB);
            question.Categories.Add(GetCategoryX());

            var categeoryRepository = Resolve<QuestionRepository>();
            categeoryRepository.Update(question);

            var questionFromDb = categeoryRepository.GetAll()[0];
            Assert.That(questionFromDb.Categories.Count, Is.EqualTo(3));
            Assert.That(questionFromDb.Categories[0].Name, Is.EqualTo("A"));
            Assert.That(questionFromDb.Categories[1].Name, Is.EqualTo("B"));
            Assert.That(questionFromDb.Categories[2].Name, Is.EqualTo("X"));
        }

        private static Category GetCategoryX()
        {
            var categoryRepository = Resolve<CategoryRepository>();
            var categoryX = new Category("X");
            categoryRepository.Create(categoryX);
            return categoryX;
        }
    }
}