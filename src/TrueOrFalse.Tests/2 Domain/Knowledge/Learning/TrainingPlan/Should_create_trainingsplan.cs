using System;
using System.Linq;
using NHibernate.Util;
using NUnit.Framework;

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
            date.TrainingPlanSettings.DebugAnswerProbabilities.Any(x => x.History.Count < 3);

        //If any of the questions has been answered less than 3 times, LearningGoalIsReached should not be marked as true
        Assert.That(!(anyQuestionAnsweredLessThan3Times && date.TrainingPlan.LearningGoalIsReached));
    }

    [Test]
    public void Should_do_update_check()
    {
        //var trainingPlan = ContextTrainingPlan.New()
        //    .Add(numberOfQuestions: 20, dateOfDate: DateTime.Now.AddDays(20))
        //    .Persist()
        //    .Last();

        //DateTimeX.Forward(days: 10);

        //RecycleContainer();

        //new TrainingPlanUpdateCheck().Execute(null);

        ////var trainingPlans = Sl.R<TrainingPlanRepo>().AllWithNewMissedDates();

        ////foreach (var trainingPlanToUpdate in trainingPlans)
        ////    TrainingPlanUpdater.Run(trainingPlanToUpdate);

        ////RecycleContainer();

        //var trainingPlanFromDb = Sl.R<TrainingPlanRepo>().GetById(trainingPlan.Id);
        //Assert.That(trainingPlanFromDb.PastDates.Count(d => !d.MarkedAsMissed), Is.EqualTo(0));
    }

    [Test]
    public void Should_add_final_boost()
    {
        var date = SetUpDateWithTrainingPlan(
            new TrainingPlanSettings
            {
                AddFinalBoost = true
            });

        var startingTimeOfLastTraining = date.TrainingPlan.Dates.Last().DateTime;
        var estimatedTimeSpanNeededForLastTraining =
            TimeSpan.FromSeconds(
                date.TrainingPlan.Dates.Last().AllQuestionsInTraining.Sum(q => q.Question.TimeToLearnInSeconds()));
        var estimatedEndTimeOfLastTraining = startingTimeOfLastTraining.Add(estimatedTimeSpanNeededForLastTraining);

        Assert.That(date.TrainingPlan.Dates.Last().AllQuestionsInTraining.Count, Is.EqualTo(date.CountQuestions()));
        Assert.That(date.DateTime.Subtract(estimatedEndTimeOfLastTraining), 
            Is.AtLeast(TimeSpan.FromHours(date.TrainingPlanSettings.NumberOfHoursLastTrainingShouldStartBeforeDate)
                        .Subtract(TimeSpan.FromMinutes(TrainingPlanSettings.TryAddDateIntervalInMinutes))));
    }

    [Test]
    public void New_Test()
    {
        var date = SetUpDateWithTrainingPlan(
            new TrainingPlanSettings
            {
                AddFinalBoost = true
            },
            timeUntilDateInDays: 16,
             numberOfQuestions: 43);

        var groupedByQuestion = date.TrainingPlan.Dates.Select(d => new {Question = d.TrainingPlan, d.AllQuestions}).GroupBy(q => q.Question).ToList();
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

        return date;
    }

    
}