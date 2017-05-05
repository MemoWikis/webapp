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
    }
}
