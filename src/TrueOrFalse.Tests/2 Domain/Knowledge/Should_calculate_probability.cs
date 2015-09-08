﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class Should_calculate_probability_ : BaseTest
    {
        [Test]
        public void Should_calculate_probability()
        {
            Assert.That(Resolve<ProbabilityCalc_Simple1>().Run(new List<AnswerHistory>{
                new AnswerHistory { AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-1) },
                new AnswerHistory { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-2) }
            }),Is.EqualTo(36));

            Assert.That(Resolve<ProbabilityCalc_Simple1>().Run(new List<AnswerHistory>{
                new AnswerHistory { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-1) },
                new AnswerHistory { AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-2) }
            }), Is.EqualTo(63));            
        }

        [Test]
        public void When_history_is_always_true_probability_should_be_100_percent()
        {
            var correctnessProbability = Resolve<ProbabilityCalc_Simple1>().Run(new List<AnswerHistory>{
                new AnswerHistory { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-1) },
                new AnswerHistory { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-2) }
            });

            Assert.That(correctnessProbability, Is.EqualTo(100));
        }

        [Test]
        public void When_history_is_always_false_correctness_probability_should_be_0_percent()
        {
            var correctnessProbability = Resolve<ProbabilityCalc_Simple1>().Run(new List<AnswerHistory>{
                new AnswerHistory{AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-1)},
                new AnswerHistory { AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-2) }
            });

            Assert.That(correctnessProbability, Is.EqualTo(0));
        }

    }
}
