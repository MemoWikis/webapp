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
            ContextQuestion.PutQuestionsIntoMemoryCache();
            Assert.That(EntityCache.GetAllQuestions().Count, Is.GreaterThan(4999));
        }

        [Test]
        public void Get_anonymous_learning_session()
        {
            var maxQuestions = 0; 
            Assert.That(ContextLearningSession.GetSteps(4000).Count, Is.EqualTo(4000));
            Assert.That(ContextLearningSession.GetSteps(6000).Count, Is.EqualTo(5000));
            Assert.That(ContextLearningSession.GetSteps(maxQuestions).Count, Is.EqualTo(5000));
        }
    }
}
