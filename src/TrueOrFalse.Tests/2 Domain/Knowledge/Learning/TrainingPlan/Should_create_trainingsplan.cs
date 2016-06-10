using System;
using System.Linq;
using NHibernate.Mapping;
using NHibernate.Util;
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
    public void Should_notify_if_learning_goal_is_not_reached()
    {
        var date = SetUpDateWithTrainingPlan(new TrainingPlanSettings
        {
            QuestionsPerDate_IdealAmount = 1,
            MinSpacingBetweenSessionsInMinutes = 1200,
        }, 
        timeUntilDateInDays: 1,
        numberOfQuestions: 20);

        var anyQuestionAnsweredLessThan3Times =
            date.TrainingPlanSettings.AnswerProbabilities.Any(x => x.History.Count < 3);

        //If any of the questions has been answered less than 3 times, LearningGoalIsReached should not be marked as true
        Assert.That(!(anyQuestionAnsweredLessThan3Times && date.TrainingPlan.LearningGoalIsReached));
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

        var realTimeBeforeUpdate = DateTime.Now;

        new TrainingPlanUpdateCheck().Execute(null);

        var trainingPlanFromDb = Sl.R<TrainingPlanRepo>().GetById(trainingPlan.Id);

        Assert.That(trainingPlanFromDb.Dates.Count(d => d.MarkedAsMissed), Is.GreaterThan(0));
        Assert.That(trainingPlanFromDb.PastDates.Count(d => !d.MarkedAsMissed), Is.EqualTo(0));
        Assert.That(trainingPlanFromDb.DateModified >= realTimeBeforeUpdate);
    }

    [Test]
    public void All_questions_should_be_boosted()
    {
        var date = SetUpDateWithTrainingPlan(
            new TrainingPlanSettings
            {
                QuestionsPerDate_IdealAmount = 10,
                AddFinalBoost = true
            },
            numberOfQuestions: 30);

        var allBoostedQuestions =
            date.TrainingPlan.Dates.Where(d => d.IsBoostingDate).SelectMany(d => d.AllQuestionsInTraining).Select(q => q.Question).OrderBy(q => q.Id).ToList();

        Assert.That(date.AllQuestions().All(q1 => allBoostedQuestions.Any(q2 => q2 == q1)));
    }

    [Test]
    public void Boosting_dates_should_start_after_normal_dates()
    {
        var date = SetUpDateWithTrainingPlan(
            new TrainingPlanSettings
            {
                QuestionsPerDate_IdealAmount = 10,
                AddFinalBoost = true
            },
            timeUntilDateInDays: 1,
            numberOfQuestions: 30);

        var boostingDatesOrdered = date.TrainingPlan.Dates.Where(d => d.IsBoostingDate).OrderBy(d => d.DateTime).ToList();
        var normalDatesOrdered = date.TrainingPlan.Dates.Except(boostingDatesOrdered).OrderBy(d => d.DateTime).ToList();

        if(normalDatesOrdered.Any() && boostingDatesOrdered.Any())

            Assert.That(boostingDatesOrdered.First().DateTime > normalDatesOrdered.Last().DateTime);
    }

    [Test]
    public void RoundingTimeShouldBeBasedOnMinutes()
    {
        //If property is based on different time interval rounding method etc. has to be adjusted 
        Assert.That(nameof(TrainingPlanSettings.TryAddDateIntervalInMinutes).ToLower().Contains("minutes"));
    }


    public Date SetUpDateWithTrainingPlan(TrainingPlanSettings settings, int timeUntilDateInDays = 30, int numberOfQuestions = 20)
    {
        var date = new Date
        {
            DateTime = DateTime.Now.AddDays(timeUntilDateInDays),
            Sets = ContextSet.New().AddSet("Set", numberOfQuestions: numberOfQuestions).All,
            User = ContextUser.GetUser()
        };

        date.TrainingPlan = TrainingPlanCreator.Run(date, settings); 

        Sl.R<DateRepo>().Create(date);

        return date;
    }

    
}