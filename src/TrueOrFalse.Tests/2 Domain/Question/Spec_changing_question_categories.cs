using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Test]
        public void Should_update_aggregated_categories()
        {
            var categoryContext = ContextCategory.New().Add("1").Add("2").Add("3").Persist();
            var categories = categoryContext.All;

            var category1 = categories[0];
            var category2 = categories[1];
            var category3 = categories[2];

            var questionContext =
                ContextQuestion.New()
                    .AddQuestion(questionText: "Question1", solutionText: "Answer", categories: new List<Category>{category1})
                    .AddQuestion(questionText: "Question2", solutionText: "Answer", categories: new List<Category> { category2 })
                    .AddQuestion(questionText: "Question3", solutionText: "Answer", categories: new List<Category> { category3 })
                    .Persist();

            ModifyRelationsForCategory.AddParentCategory(category2, category1);
            ModifyRelationsForCategory.AddParentCategory(category3, category2);

            categoryContext.Update();

            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category1);
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category2);
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category3);

            Assert.That(Sl.QuestionRepo.GetForCategory(category1.Id).Count == 1);
            Assert.That(category1.GetAggregatedContent().AggregatedQuestions.Count == 3);
            Assert.That(category1.GetAggregatedContent().AggregatedQuestions.Any(q => q.Text == "Question3"));

            Assert.That(Sl.QuestionRepo.GetForCategory(category2.Id).Count == 1);
            Assert.That(category2.GetAggregatedContent().AggregatedQuestions.Count == 2);
            Assert.That(category2.GetAggregatedContent().AggregatedQuestions.Any(q => q.Text == "Question3"));

            Assert.That(Sl.QuestionRepo.GetForCategory(category3.Id).Count == 1);
            Assert.That(category3.GetAggregatedContent().AggregatedQuestions.Count == 1);
            Assert.That(category3.GetAggregatedContent().AggregatedQuestions.Any(q => q.Text == "Question3"));

            var question3 = questionContext.All[2];

            question3.Categories.Remove(category3);

            Sl.QuestionRepo.Update(question3);

            Assert.That(Sl.QuestionRepo.GetForCategory(category1.Id).Count == 1);
            Assert.That(category1.GetAggregatedContent().AggregatedQuestions.Count == 2);
            Assert.That(category1.GetAggregatedContent().AggregatedQuestions.All(q => q.Text != "Question3"));

            Assert.That(Sl.QuestionRepo.GetForCategory(category2.Id).Count == 1);
            Assert.That(category2.GetAggregatedContent().AggregatedQuestions.Count == 1);
            Assert.That(category2.GetAggregatedContent().AggregatedQuestions.All(q => q.Text != "Question3"));

            Assert.That(Sl.QuestionRepo.GetForCategory(category3.Id).Count == 0);
            Assert.That(category3.GetAggregatedContent().AggregatedQuestions.Count == 0);
        }

    }
}