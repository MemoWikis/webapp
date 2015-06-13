using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Set_persistence : BaseTest
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

            var questionSet = new Set {Creator = ContextUser.New().Add("some body").Persist().All.Last()};
            questionSet.Add(context.All);

            Resolve<SetRepo>().Create(questionSet);

            var questionSetFromDb = Resolve<SetRepo>().GetAll()[0];
            Assert.That(questionSetFromDb.QuestionsInSet.Count, Is.EqualTo(4));
        }
    }
}
