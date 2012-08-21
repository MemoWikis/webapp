using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Integration)]
    public class Should_recalculate_all_wish_knowledge_items : BaseTest
    {
        [Test]
        public void Run()
        {
            var contextQuestion = ContextQuestion.New()
                .AddQuestion("QuestionA", "AnswerA").AddCategory("A")
                .Persist();

            var questionId = contextQuestion.Questions.First().Id;
            var updateTotals = Resolve<UpdateQuestionTotals>();

            updateTotals.Run(new QuestionValuation { RelevancePersonal = 100, QuestionId = questionId, UserId = 2 });
            updateTotals.Run(new QuestionValuation { RelevancePersonal = 1, QuestionId = questionId, UserId = 2 });
            updateTotals.Run(new QuestionValuation { QuestionId = questionId, UserId = 2 });

            Resolve<RecalculateAllKnowledgeItems>().Run();
            Assert.That(Resolve<GetWishKnowledgeCount>().Run(userId: 2), Is.EqualTo(2));
        }
    }
}
