using System.Collections.Generic;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class QestionValuation_persistence_tests : BaseTest
    {
        [Test]
        public void QuestionValuation_should_be_persisted()
        {
            var contextQuestion = ContextQuestion.New().AddQuestion("a", "b").Persist();
            var questionValuation = 
                new QuestionValuation
                {
                    QuestionId = contextQuestion.All[0].Id,
                    UserId = 1,
                    Quality = 91,
                    RelevanceForAll = 40,
                    RelevancePersonal = 7
                };

            Resolve<QuestionValuationRepository>().Create(questionValuation);
        }

        [Test]
        public void Should_select_by_question_ids()
        {
            var context = ContextQuestion.New()
                .AddQuestion("1", "a")
                .AddQuestion("2", "a")
                .AddQuestion("3", "a")
                .AddQuestion("4", "a")
                .AddQuestion("5", "a")
                .Persist();

            var questionValuation1 = new QuestionValuation { QuestionId = context.All[0].Id, UserId = 1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation2 = new QuestionValuation { QuestionId = context.All[1].Id, UserId = 1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation3 = new QuestionValuation { QuestionId = context.All[2].Id, UserId = 2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation4 = new QuestionValuation { QuestionId = context.All[3].Id, UserId = 1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation5 = new QuestionValuation { QuestionId = context.All[4].Id, UserId = 2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };

            Resolve<QuestionValuationRepository>().Create(
                new List<QuestionValuation> { questionValuation1, questionValuation2, questionValuation3, questionValuation4, questionValuation5 });

            Assert.That(Resolve<QuestionValuationRepository>().GetBy(
                new[] { context.All[0].Id, context.All[1].Id, context.All[2].Id }, 1).Count, Is.EqualTo(2));
        }
    }
}
