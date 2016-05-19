using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Utilities.ScheduledJobs;
using EnumerableExtensions = NHibernate.Util.EnumerableExtensions;

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

        RecycleContainer();

        var trainingPlans = Sl.R<TrainingPlanRepo>().AllWithNewMissedDates();

        foreach (var trainingPlanToUpdate in trainingPlans)
            TrainingPlanUpdater.Run(trainingPlanToUpdate);

        RecycleContainer();

        var trainingPlanFromDb = Sl.R<TrainingPlanRepo>().GetById(trainingPlan.Id);
        Assert.That(trainingPlanFromDb.PastDates.Count(d => !d.MarkedAsMissed), Is.EqualTo(0));
    }

    [Test]
    public void Should_add_final_boost()
    {
        var date = new Date
        {
            DateTime = DateTime.Now.AddDays(30),
            Sets = ContextSet.New().AddSet("Set", numberOfQuestions: 30).All,
            User = ContextUser.GetUser()
        };

        date.TrainingPlan = TrainingPlanCreator.Run(
            date,
            new TrainingPlanSettings
            {
                AddFinalBoost = false,
                SpacingBetweenSessionsInMinutes = 100
            });

        var lastDate = date.TrainingPlan.Dates.Last();
        var newTimeOfLastDate = date.DateTime.AddMinutes(-(date.TrainingPlanSettings.SpacingBetweenSessionsInMinutes - 10));
        lastDate.DateTime = newTimeOfLastDate;

        foreach (var answerProbability in date.TrainingPlanSettings.DebugAnswerProbabilities)
        {
            foreach (var historyItem in answerProbability.History)
            {
                if (historyItem.TrainingDate == lastDate)
                    historyItem.Answer.DateCreated = newTimeOfLastDate;
            }
        }

       // Assert.That(!date.TrainingPlan.Dates.Any(d => d.DateTime > ));

        TrainingPlanCreator.T_AddFinalBoost(
            date, 
            date.TrainingPlanSettings,
            date.TrainingPlan.Settings.DebugAnswerProbabilities,
            date.TrainingPlan.Dates.ToList());
    }
}