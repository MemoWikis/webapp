using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests.Persistence
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
                    RelevancePesonal = 7
                };

            Resolve<QuestionValuationRepository>().Create(questionValuation);
        }
    }
}
