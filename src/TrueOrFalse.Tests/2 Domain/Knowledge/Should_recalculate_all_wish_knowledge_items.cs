using System.Linq;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Integration)]
    public class Should_recalculate_all_wish_knowledge_items : BaseTest
    {
        [Test]
        public void Run()
        {
            var updateTotals = Resolve<UpdateQuestionTotals>();

            updateTotals.Run(new QuestionValuation { RelevancePersonal = 100, QuestionId = 1, UserId = 2 });
            updateTotals.Run(new QuestionValuation { RelevancePersonal = 1, QuestionId = 2, UserId = 2 });
            updateTotals.Run(new QuestionValuation { QuestionId = 3, UserId = 2 });

            Resolve<ISession>().Flush();

            Resolve<RecalculateAllKnowledgeItems>().Run();
            Assert.That(Resolve<GetWishKnowledgeCount>().Run(userId: 2), Is.EqualTo(2));
        }
    }
}
