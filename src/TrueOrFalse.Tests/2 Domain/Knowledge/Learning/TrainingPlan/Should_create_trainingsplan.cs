using System;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Utilities.ScheduledJobs;

public class Should_create_trainingsplan : BaseTest
{
    [Test]
    public void With_no_history()
    {
        var trainingsPlan = ContextTrainingPlan.New()
            .Add(numberOfQuestions: 20, dateOfDate: DateTime.Now.AddDays(7))
            .Last();

        Assert.That(trainingsPlan.Dates.Count, Is.InRange(5, 15));

        trainingsPlan.DumpToConsole();
    }

    [Test]
    public void When_not_trained__in_the_remaining_time_the_dates_amount_should_increase()
    {
        var trainingPlan = ContextTrainingPlan.New()
            .Add(numberOfQuestions: 20, dateOfDate: DateTime.Now.AddDays(20))
            .Persist()
            .Last();

        var amountOfDatesInsLast7Days = trainingPlan.Dates.Count(d => d.DateTime > DateTime.Now.AddDays(10));

        DateTimeX.Forward(days: 10);

        trainingPlan = TrainingPlanUpdater.Run(trainingPlan.Id);

        Assert.That(amountOfDatesInsLast7Days, Is.LessThan(trainingPlan.OpenDates.Count));

        RecycleContainer();

        var allDates = R<TrainingDateRepo>().GetAll();
        Assert.That(allDates[0].TrainingPlan, Is.EqualTo(trainingPlan));
    }

    [Test]
    public void Should_do_update_check()
    {
        var trainingPlan = ContextTrainingPlan.New()
            .Add(numberOfQuestions: 20, dateOfDate: DateTime.Now.AddDays(20))
            .Persist()
            .Last();

        DateTimeX.Forward(days: 10);

        var dateTimeNow = DateTimeX.Now();

        var date = trainingPlan.Date;

        var trainingPlans = Sl.R<TrainingPlanRepo>().AllWithNewMissedDates();

        foreach (var trainingPlanToUpdate in trainingPlans)
        {
            TrainingPlanUpdater.Run(trainingPlanToUpdate);
        }

        var x = trainingPlan.Dates.Where(d => d.DateTime < dateTimeNow).ToList();

        Assert.That(trainingPlan.Dates.Count(d => d.DateTime < dateTimeNow && !d.MarkedAsMissed), Is.EqualTo(0));
    }
}