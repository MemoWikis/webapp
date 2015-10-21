using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Set_persistence : BaseTest
    {
        [Test]
        public void QuestionSet_should_be_persisted()
        {
            var questionContext = ContextQuestion.New()
                .AddQuestion(questionText: "Q1", solutionText: "A1").AddCategory("A")
                .AddQuestion(questionText: "Q2", solutionText: "A2").AddCategory("A")
                .AddQuestion(questionText: "Q3", solutionText: "A3")
                .AddQuestion(questionText: "Q4", solutionText: "A4").AddCategory("B")
                .Persist();

            var set = new Set { Creator = ContextUser.GetUser() };
            set.Add(questionContext.All);

            R<SetRepo>().Create(set);

            RecycleContainer();

            var questionSetFromDb = R<SetRepo>().GetAll()[0];
            Assert.That(questionSetFromDb.QuestionsInSet.Count, Is.EqualTo(4));
        }
    }
}
