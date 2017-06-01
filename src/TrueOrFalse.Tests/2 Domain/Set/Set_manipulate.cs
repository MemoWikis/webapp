using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class Set_manipulate : BaseTest
    {
        [Test]
        public void Should_add_question_to_questionSet()
        {        
            var context = ContextQuestion.New()
                .AddQuestion(questionText: "Q1", solutionText: "A1").AddCategory("A")
                .AddQuestion(questionText: "Q2", solutionText: "A2").AddCategory("A")
                .Persist();

            var questionSetRepo = Resolve<SetRepo>();
            var questionSet = new Set {Name = "QS1", Creator = ContextUser.New().Add("some user").Persist().All.Last()};
            questionSetRepo.Create(questionSet);

            AddToSet.Run(context.All.GetIds().ToArray(), questionSet.Id);

            base.RecycleContainer();
            var questionSetFromDb = Resolve<SetRepo>().GetById(questionSet.Id);
            var questionSetFromDb2 = Resolve<ISession>().QueryOver<Set>().SingleOrDefault();

            Assert.That(questionSetFromDb2.QuestionsInSet.Count, Is.EqualTo(2));
            Assert.That(questionSetFromDb.QuestionsInSet.Count, Is.EqualTo(2));
            Assert.That(Resolve<QuestionInSetRepo>().GetAll().Count, Is.EqualTo(2));
            Assert.That(Resolve<QuestionInSetRepo>().GetAll().First().Set.Id, Is.EqualTo(1));

            var allQuestions = Resolve<QuestionRepo>().GetAll();
            Assert.That(allQuestions[0].SetsAmount, Is.EqualTo(1));
            Assert.That(allQuestions[0].SetTop5Minis[0].Id, Is.EqualTo(1));
            Assert.That(allQuestions[0].SetTop5Minis[0].Name, Is.EqualTo("QS1"));
        }

        [Test]
        public void Should_add_categories_to_set()
        {
            var context = ContextSet.New()
                .AddSet("Some question set")
                    .AddQuestion("some question 1", "some solution 1")
                    .AddQuestion("some question 2", "some solution 2")
                    .AddCategory("category 1")
                    .AddCategory("category 2")
                .Persist();

            RecycleContainer();

            var setRepo = Resolve<SetRepo>();
            var setFromDb = setRepo.GetById(context.All.Last().Id);

            Assert.That(setFromDb.QuestionsInSet.Count, Is.EqualTo(2));
            Assert.That(setFromDb.Categories.Count, Is.EqualTo(2));
            Assert.That(setFromDb.Categories.Count(x => x.Name == "category 1"), Is.EqualTo(1));
        }

        [Test]
        public void Should_update_aggregated_categories()
        {
            var categoryContext = ContextCategory.New().Add("1").Add("2").Add("3").Persist();
            var categories = categoryContext.All;

            var category1 = categories[0];
            var category2 = categories[1];
            var category3 = categories[2];

            var setContext =
                ContextSet.New()
                    .AddSet(name: "Set1", categories: new List<Category> { category1 })
                    .AddSet(name: "Set2", categories: new List<Category> { category2 })
                    .AddSet(name: "Set3", categories: new List<Category> { category3 })
                    .Persist();

            ModifyRelationsForCategory.AddParentCategory(category2, category1);
            ModifyRelationsForCategory.AddParentCategory(category3, category2);

            categoryContext.Update();

            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category1);
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category2);
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category3);

            Assert.That(Sl.SetRepo.GetForCategory(category1.Id).Count == 1);
            Assert.That(category1.GetAggregatedContent().AggregatedSets.Count == 3);
            Assert.That(category1.GetAggregatedContent().AggregatedSets.Any(s => s.Name == "Set3"));

            Assert.That(Sl.SetRepo.GetForCategory(category2.Id).Count == 1);
            Assert.That(category2.GetAggregatedContent().AggregatedSets.Count == 2);
            Assert.That(category2.GetAggregatedContent().AggregatedSets.Any(s => s.Name == "Set3"));

            Assert.That(Sl.SetRepo.GetForCategory(category3.Id).Count == 1);
            Assert.That(category3.GetAggregatedContent().AggregatedSets.Count == 1);
            Assert.That(category3.GetAggregatedContent().AggregatedSets.Any(s => s.Name == "Set3"));

            var set3 = setContext.All[2];

            set3.Categories.Remove(category3);

            Sl.SetRepo.Update(set3);

            Assert.That(Sl.SetRepo.GetForCategory(category1.Id).Count == 1);
            Assert.That(category1.GetAggregatedContent().AggregatedSets.Count == 2);
            Assert.That(category1.GetAggregatedContent().AggregatedSets.All(s => s.Name != "Question3"));

            Assert.That(Sl.SetRepo.GetForCategory(category2.Id).Count == 1);
            Assert.That(category2.GetAggregatedContent().AggregatedSets.Count == 1);
            Assert.That(category2.GetAggregatedContent().AggregatedSets.All(s => s.Name != "Question3"));

            Assert.That(Sl.SetRepo.GetForCategory(category3.Id).Count == 0);
            Assert.That(category3.GetAggregatedContent().AggregatedSets.Count == 0);
        }
    }
}
