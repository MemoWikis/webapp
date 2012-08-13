using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class CorrectnessProbabilityCalculatorTests : BaseTest
    {
        [Test]
        public void Should_calculate_correctness()
        {
            Assert.That(Resolve<CorrectnessProbabilityCalculator>().Run(new List<AnswerHistory>{
                new AnswerHistory { AnswerredCorrectly = false, DateCreated = DateTime.Now.AddDays(-1) },
                new AnswerHistory { AnswerredCorrectly = true, DateCreated = DateTime.Now.AddDays(-2) }
            }),Is.EqualTo(36));

            Assert.That(Resolve<CorrectnessProbabilityCalculator>().Run(new List<AnswerHistory>{
                new AnswerHistory { AnswerredCorrectly = true, DateCreated = DateTime.Now.AddDays(-1) },
                new AnswerHistory { AnswerredCorrectly = false, DateCreated = DateTime.Now.AddDays(-2) }
            }), Is.EqualTo(63));            
        }

        [Test]
        public void When_history_is_always_true_correctness_probability_should_be_100_percent()
        {
            var correctnessProbability = Resolve<CorrectnessProbabilityCalculator>().Run(new List<AnswerHistory>{
                new AnswerHistory { AnswerredCorrectly = true, DateCreated = DateTime.Now.AddDays(-1) },
                new AnswerHistory { AnswerredCorrectly = true, DateCreated = DateTime.Now.AddDays(-2) }
            });

            Assert.That(correctnessProbability, Is.EqualTo(100));
        }

        [Test]
        public void When_history_is_always_false_correctness_probability_should_be_0_percent()
        {
            var correctnessProbability = Resolve<CorrectnessProbabilityCalculator>().Run(new List<AnswerHistory>{
                new AnswerHistory{AnswerredCorrectly = false, DateCreated = DateTime.Now.AddDays(-1)},
                new AnswerHistory { AnswerredCorrectly = false, DateCreated = DateTime.Now.AddDays(-2) }
            });

            Assert.That(correctnessProbability, Is.EqualTo(0));
        }

    }
}
