using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_create_learningSession_steps : BaseTest
{
    [Test]
    public void Should_select_and_order_questions_correctly()
    {
        var context = ContextQuestion.New();
        var learner = context.Learner;

        Sl.R<SessionUser>().Login(learner);

        context.PersistImmediately()
            .AddQuestion("question not answered 1")
            .AddQuestion("question not answered 2")
            .AddQuestion("question not answered 3")
            .AddQuestion("question answered not today prob 20")
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(20, learner)
            .AddQuestion("question answered not today prob 40")//1
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(40, learner)
            .AddQuestion("question answered not today prob 30")//1
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(30, learner)
            .AddQuestion("question answered today")
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now)
                .AddAnswers(countCorrect: 7, countWrong: 3, dateCreated: DateTime.Now.AddDays(-1))
                .SetProbability(20, learner);

        RecycleContainer();

        const int numberOfSteps = 10;
        var rnd = new Random();
        Console.WriteLine("Original order:" + Environment.NewLine
            + context.All.Select(s => s.Text).Aggregate((current, next) => current + Environment.NewLine + next));

        context.All = context.All.OrderBy(x => rnd.Next()).ToList();
        Console.WriteLine(Environment.NewLine + "Shuffled:" + Environment.NewLine
            + context.All.Select(s => s.Text).Aggregate((current, next) => current + Environment.NewLine + next));
        
        var steps = GetLearningSessionSteps.Run(context.All, numberOfSteps);

        Console.WriteLine(Environment.NewLine + "Selected steps:" + Environment.NewLine
            + steps.Select(s => s.Question.Text).Aggregate((current, next) => current + Environment.NewLine + next));

        Assert.That(steps[0].Question.Text.StartsWith("question not answered"));
        Assert.That(steps[1].Question.Text.StartsWith("question not answered"));
        Assert.That(steps[2].Question.Text.StartsWith("question not answered"));
        Assert.That(steps[3].Question.Text == "question answered not today prob 20");
        Assert.That(steps[4].Question.Text == "question answered not today prob 30");
        Assert.That(steps[5].Question.Text == "question answered not today prob 40");
        Assert.That(steps[6].Question.Text == "question answered today");

        var learningSession = new LearningSession();
        R<LearningSessionRepo>().Create(learningSession);
        learningSession.Steps = steps;
        R<LearningSessionRepo>().Update(learningSession);

        RecycleContainer();

        R<AnswerQuestion>().Run(steps[0].Question.Id, "answer1", -1, steps[0].Id);
        R<AnswerQuestion>().Run(steps[1].Question.Id, "answer2", -1, steps[1].Id);

        RecycleContainer();

        var learningSessionFromDb = R<LearningSessionRepo>().GetById(learningSession.Id);

        if (IsMysqlInMemoryEngine())
            learningSessionFromDb.Steps = learningSessionFromDb.Steps.OrderBy(s => s.Id).ToList();

        Assert.That(learningSessionFromDb.Steps[0].AnswerHistory.AnswerText, Is.EqualTo("answer1"));
        Assert.That(learningSessionFromDb.Steps[1].AnswerHistory.AnswerText == "answer2");
        Assert.That(learningSessionFromDb.Steps[2].AnswerHistory == null);
    }
}