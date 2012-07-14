using System.Collections.Generic;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class QestionValuation_persistence_tests : BaseTest
    {
        [Test]
        public void QuestionValuation_should_be_persisted()
        {
            var questionValuation = 
                new QuestionValuation
                {
                    QuestionId = 1,
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
            var questionValuation1 = new QuestionValuation { QuestionId = 1, UserId = 1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation2 = new QuestionValuation { QuestionId = 2, UserId = 1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation3 = new QuestionValuation { QuestionId = 3, UserId = 2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation4 = new QuestionValuation { QuestionId = 4, UserId = 1, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };
            var questionValuation5 = new QuestionValuation { QuestionId = 5, UserId = 2, Quality = 91, RelevanceForAll = 40, RelevancePersonal = 7 };

            Resolve<QuestionValuationRepository>().Create(
                new List<QuestionValuation> { questionValuation1, questionValuation2, questionValuation3, questionValuation4, questionValuation5 });

            Assert.That(Resolve<QuestionValuationRepository>().GetBy(new[] {1, 2, 3}, 1).Count, Is.EqualTo(2));
        }
    }
}
