using System;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_create_learningSession_steps : BaseTest
{
    [Test]
    public void Should_select_and_order_questions_correctly()
    {
        var context = ContextQuestion.New();
        var learner = context.Learner;

        context.PersistImmediately()
            .AddQuestion()
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now)
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(20, learner)
            .AddQuestion()
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now)
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(20, learner);

        RecycleContainer();
    }
}