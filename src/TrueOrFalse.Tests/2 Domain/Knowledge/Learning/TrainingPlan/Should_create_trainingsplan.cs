using System;
using NUnit.Framework;

public class Should_create_trainingsplan : BaseTest
{
    [Test]
    public void With_no_history()
    {
        var trainingsPlan = ContextTrainingPlan.New()
            .Add(numberOfQuestions:20, dateOfDate:DateTime.Now.AddDays(7))
            .Last();

        Assert.That(trainingsPlan.Dates.Count, Is.InRange(5, 35));

        trainingsPlan.DumpToConsole();
    }

    [Test]
    public void Update_plan()
    {
        var trainingsPlan = ContextTrainingPlan.New()
            .Add(numberOfQuestions: 20, dateOfDate: DateTime.Now.AddDays(7))
            .Last();
    }
}