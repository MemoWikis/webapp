using System;
using System.Linq;
using NUnit.Framework;

public class Should_create_trainingsplan : BaseTest
{
    [Test]
    public void With_no_history()
    {
        var trainingsPlan = ContextTrainingPlan.New()
            .Add(numberOfQuestions:20, dateOfDate:DateTime.Now.AddDays(7))
            .Last();

        Assert.That(trainingsPlan.Dates.Count, Is.InRange(5, 15));

        trainingsPlan.DumpToConsole();
    }

    [Test]
    public void When_not_trained__in_the_remaining_time_the_dates_amount_should_increase()
    {
        var trainingsPlan = ContextTrainingPlan.New()
            .Add(numberOfQuestions: 20, dateOfDate: DateTime.Now.AddDays(20))
            .Persist()
            .Last();

        var amountOfDatesInsLast7Days = trainingsPlan.Dates.Count(d => d.DateTime > DateTime.Now.AddDays(10));

        DateTimeX.Forward(days:10);

        var updatedTrainingPlan = TrainingPlanUpdater.Run(trainingsPlan.Id);

        Assert.That(amountOfDatesInsLast7Days, Is.LessThan(updatedTrainingPlan.DatesInFuture.Count));

        trainingsPlan.DumpToConsole();
        updatedTrainingPlan.DumpToConsole();
    }
}