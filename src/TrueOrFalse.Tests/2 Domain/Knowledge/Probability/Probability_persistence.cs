using System;
using System.Collections.Generic;
using Autofac;
using NHibernate;
using NUnit.Framework;
using SharpTestsEx;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.Persistence
{
    [Category(TestCategories.Programmer)]
    public class Probability_persistence : BaseTest
    {
        [Test]
        public void Should_persist()
        {
            var questionContext = ContextQuestion.New()
                .AddQuestion("A1", "B2")
                .AddQuestion("A2", "B2")
                .Persist();

            ContextProbability.New()
                .Add(questionContext.All)
                .Persist();

            var probabilityRepo = Resolve<ProbabilityRepo>();
            Assert.That(probabilityRepo.GetAll().Count, Is.EqualTo(2));
        }
    }
}
