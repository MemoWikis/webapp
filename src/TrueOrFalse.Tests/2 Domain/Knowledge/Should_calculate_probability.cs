using System;
using System.Collections.Generic;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_calculate_probability_ : BaseTest
{
    [Test]
    public void Should_calculate_probability()
    {
        var user = ContextUser.New().Add("User").Persist().All[0];
        var userCacheItem = UserCacheItem.ToCacheUser(user);
        var question = ContextQuestion.New().AddQuestion(questionText: "question").Persist().All[0];

        Assert.That(Resolve<ProbabilityCalc_Simple1>().Run(new List<Answer>{
            new Answer { AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-1) },
            new Answer { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-2) }
        },QuestionCacheItem.ToCacheQuestion(question), userCacheItem).Probability,Is.EqualTo(36));

        Assert.That(Resolve<ProbabilityCalc_Simple1>().Run(new List<Answer>{
            new Answer { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-1) },
            new Answer { AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-2) }
        }, QuestionCacheItem.ToCacheQuestion(question), userCacheItem).Probability, Is.EqualTo(63));            
    }

    [Test]
    public void When_history_is_always_true_probability_should_be_100_percent()
    {
        var user = ContextUser.New().Add("User").Persist().All[0];
        var userCacheItem = UserCacheItem.ToCacheUser(user);
        var question = ContextQuestion.New().AddQuestion(questionText: "question").Persist().All[0];

        var correctnessProbability = Resolve<ProbabilityCalc_Simple1>().Run(new List<Answer>{
            new Answer { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-1) },
            new Answer { AnswerredCorrectly = AnswerCorrectness.True, DateCreated = DateTime.Now.AddDays(-2) }
        }, QuestionCacheItem.ToCacheQuestion(question), userCacheItem);

        Assert.That(correctnessProbability.Probability, Is.EqualTo(100));
    }

    [Test]
    public void When_history_is_always_false_correctness_probability_should_be_0_percent()
    {
        var user = ContextUser.New().Add("User").Persist().All[0];
        var userCacheItem = UserCacheItem.ToCacheUser(user);
        var question = ContextQuestion.New().AddQuestion(questionText: "question").Persist().All[0];

        var correctnessProbability = Resolve<ProbabilityCalc_Simple1>().Run(new List<Answer>{
            new Answer{AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-1)},
            new Answer { AnswerredCorrectly = AnswerCorrectness.False, DateCreated = DateTime.Now.AddDays(-2) }
        }, QuestionCacheItem.ToCacheQuestion(question), userCacheItem);

        Assert.That(correctnessProbability.Probability, Is.EqualTo(0));
    }

}