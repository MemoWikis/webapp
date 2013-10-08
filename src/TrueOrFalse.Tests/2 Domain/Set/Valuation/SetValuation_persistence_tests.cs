using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    [Category(TestCategories.Programmer)]
    public class SetValuation_persistence_tests : BaseTest
    {
        [Test]
        public void SetValuation_should_be_persisted()
        {
            ContextSet.New().AddSet("1").Persist();
            var questionValuation =
                new SetValuation{
                    SetId = 1,
                    UserId = 1,
                    RelevancePersonal = 10
                };

            Resolve<SetValuationRepository>().Create(questionValuation);
        }

        [Test]
        public void Should_select_by_user_and_setIds()
        {
            ContextSet.New().AddSet("1").AddSet("2").AddSet("3").Persist();
            var setValuation1 = new SetValuation { SetId = 1, UserId = 1, RelevancePersonal = 7 };
            var setValuation2 = new SetValuation { SetId = 2, UserId = 1, RelevancePersonal = 7 };
            var setValuation3 = new SetValuation { SetId = 3, UserId = 2, RelevancePersonal = 7 };

            Resolve<SetValuationRepository>().Create(new List<SetValuation>{setValuation1, setValuation2, setValuation3});

            Assert.That(Resolve<SetValuationRepository>().GetBy(new[] { 1, 2, 3 }, 1).Count, Is.EqualTo(2));
        }

        [Test]
        public void Should_get_wish_count_for_user()
        {
            ContextSet.New().AddSet("1").AddSet("2").AddSet("3").Persist();
            var setValuation1 = new SetValuation { SetId = 1, UserId = 1, RelevancePersonal = 7 };
            var setValuation2 = new SetValuation { SetId = 2, UserId = 1, RelevancePersonal = 7 };
            var setValuation3 = new SetValuation { SetId = 3, UserId = 1, RelevancePersonal = -1 };

            Resolve<SetValuationRepository>().Create(new List<SetValuation>{setValuation1, setValuation2, setValuation3});

            Assert.That(Resolve<GetWishSetCount>().Run(1), Is.EqualTo(2));
            Assert.That(Resolve<GetWishSetCount>().Run(2), Is.EqualTo(0));
        }
    }
}