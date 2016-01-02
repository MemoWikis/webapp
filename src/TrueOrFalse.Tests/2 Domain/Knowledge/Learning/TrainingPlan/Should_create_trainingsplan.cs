using System;
using NUnit.Framework;

public class Should_create_trainingsplan : BaseTest
{
    [Test]
    public void With_no_history()
    {
        var contextSet = ContextSet.New().AddSet("Setname", numberOfQuestions: 20).Persist();
        var contextDate = ContextDate.New().Add(contextSet.All, dateTime: DateTime.Now.AddDays(7) ).Persist();

        var trainingsPlan = TrainingPlanCreator.Run(
            contextDate.All[0], new TrainingPlanSettings{
                QuestionsPerDate_IdealAmount = 5,
                QuestionsPerDate_Minimum = 3
            });

        Assert.That(trainingsPlan.Dates.Count, Is.InRange(5, 35));

        trainingsPlan.DumpToConsole();
    }
}