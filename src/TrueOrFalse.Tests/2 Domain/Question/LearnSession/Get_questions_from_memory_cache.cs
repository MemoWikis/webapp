using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    class Get_questions_from_memory_cache : BaseTest
    {
        [Test]
        public void Should_store_questions_into_memory_cache()
        {
            ContextQuestion.PutQuestionsIntoMemoryCache(5000);
            Assert.That(EntityCache.GetAllQuestions().Count, Is.GreaterThan(4999));
        }

        [Test]
        public void Get_anonymous_learning_session()
        {
            Assert.That(ContextLearningSession.GetSteps(4000, 4000).Count, Is.EqualTo(4000));
        }
    }
}
