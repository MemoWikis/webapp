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
            var context = ContextQuestion.New()
                .AddQuestion("1", "")
                .AddQuestion("2", "")
                .AddQuestion("3", "")
                .Persist();

            var updateTotals = Resolve<UpdateQuestionTotals>();
            updateTotals.Run(new QuestionValuation { RelevancePersonal = 100, QuestionId = context.All[0].Id, UserId = 2 });
            updateTotals.Run(new QuestionValuation { RelevancePersonal = 1, QuestionId = context.All[1].Id, UserId = 2 });
            updateTotals.Run(new QuestionValuation { QuestionId = context.All[2].Id, UserId = 2 });

            Resolve<ISession>().Flush();

            Resolve<ProbabilityForUsersUpdate>().Run();
            Assert.That(Resolve<GetWishQuestionCount>().Run(userId: 2), Is.EqualTo(2));

            Resolve<ISession>().Flush();

            var summary = Resolve<KnowledgeSummaryLoader>().Run(userId: 2);
            Assert.That(summary.Total, Is.EqualTo(2));
            Assert.That(summary.Unknown, Is.EqualTo(2));
        }
    }
}
