using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate.Criterion;
using TrueOrFalse;

public class TrainingPlanCreator
{
    public const int IntervalInMinutes = 15;

    public static int[] QuestionsToTrackIds = {};

    public static TrainingPlan Run(Date date, TrainingPlanSettings settings)
    {
        var trainingPlan = new TrainingPlan();
        trainingPlan.Date = date;
        trainingPlan.Settings = settings;

        if (date.AllQuestions().Count == 0)
            return trainingPlan;

        if (date.AllQuestions().Count < settings.QuestionsPerDate_Minimum)
            settings.QuestionsPerDate_Minimum = date.AllQuestions().Count;

        if (settings.QuestionsPerDate_IdealAmount < settings.QuestionsPerDate_Minimum)
            settings.QuestionsPerDate_IdealAmount = settings.QuestionsPerDate_Minimum;

        settings.DebugAnswerProbabilities = GetInitialAnswerProbabilities(date);

        trainingPlan.Dates = GetDates(date, settings);

        var probabilitiesAtTimeOfDate = ReCalcAllAnswerProbablities(date.DateTime, settings.DebugAnswerProbabilities);
        trainingPlan.LearningGoalIsReached = probabilitiesAtTimeOfDate.All(p => p.CalculatedProbability >= settings.AnswerProbabilityThreshold);

        return trainingPlan;
    }

    private static List<AnswerProbability> GetInitialAnswerProbabilities(Date date)
    {
        var probUpdateValRepo = Sl.R<ProbabilityUpdate_Valuation>();

        foreach (var question in date.AllQuestions())
        {
            probUpdateValRepo.Run(question.Id, date.User.Id);
        }

        var answerRepo = Sl.R<AnswerRepo>();
        var questionValuationRepo = Sl.R<QuestionValuationRepo>();

        return date
                .AllQuestions()
                .Select(q =>
                    new AnswerProbability
                    {
                        User = date.User,
                        Question = q,
                        CalculatedProbability = questionValuationRepo.GetBy(q.Id, date.User.Id).KnowledgeStatus.GetProbability(q.Id),
                        CalculatedAt = DateTimeX.Now(),
                        History = answerRepo
                            .GetByQuestion(q.Id, date.User.Id).Select(x => new AnswerProbabilityHistory(x, null))
                            .ToList()
                    })
                .ToList();
    }

    private static IList<TrainingDate> GetDates(Date date, TrainingPlanSettings settings)
    {
        var nextDateProposal = DateTimeUtils.RoundUp(DateTimeX.Now(), TimeSpan.FromMinutes(IntervalInMinutes));

        var learningDates = new List<TrainingDate>();

        while (nextDateProposal < date.DateTime)
        {
            nextDateProposal = nextDateProposal.AddMinutes(IntervalInMinutes);

            if (settings.IsInSnoozePeriod(nextDateProposal))
                continue;

            if(TryAddDate(settings, nextDateProposal, learningDates))
                nextDateProposal = nextDateProposal.AddMinutes(date.TrainingPlanSettings.SpacingBetweenSessionsInMinutes
                    //- IntervalInMinutes
                    );
        }

        if (settings.AddFinalBoost)
        {
            AddFinalBoost(date, settings, learningDates);
        }

        return learningDates;
    }

    private static bool TryAddDate(
        TrainingPlanSettings settings,
        DateTime proposedDateTime,
        List<TrainingDate> learningDates)
    {
        var newAnswerProbabilities = 
            ReCalcAllAnswerProbablities(proposedDateTime, settings.DebugAnswerProbabilities);

        var belowThresholdCount = newAnswerProbabilities.Count(x => x.CalculatedProbability < settings.AnswerProbabilityThreshold);

        if (settings.DebugAnswerProbabilities.Count(x => x.History.Count > 0 && x.CalculatedProbability < 15) > 0)
            Debugger.Break();

        //if((proposedDateTime.Hour % 4) == 0 && proposedDateTime.Minute < 15)
        //    answerProbabilities.Log();

        if (belowThresholdCount < settings.QuestionsPerDate_Minimum)
            return false;

        settings.DebugAnswerProbabilities = newAnswerProbabilities.ToList();

        var trainingDate = new TrainingDate{DateTime = proposedDateTime};
        learningDates.Add(trainingDate);

        trainingDate.AllQuestions =
            settings.DebugAnswerProbabilities
                .OrderBy(x => x.CalculatedProbability)
                .ThenBy(x => x.History.Count)
                .Select(x => new TrainingQuestion
                {
                    Question = x.Question,
                    ProbBefore = x.CalculatedProbability,
                    ProbAfter = x.CalculatedProbability
                })
                .ToList();

        for (int i = 0; i < belowThresholdCount; i++)
        {
            var trainingQuestion = trainingDate.AllQuestions[i];
            trainingQuestion.IsInTraining = true;
            /*directly after training the probability is almost 100%!*/
            trainingQuestion.ProbAfter = 99;

            settings.DebugAnswerProbabilities
                .By(trainingQuestion.Question.Id)
                .AddAnswerAndSetProbability(trainingQuestion.ProbAfter, trainingDate.DateTime, trainingDate);

            if (settings.QuestionsPerDate_IdealAmount < i + 2)
                break;
        }

        var probs = settings.DebugAnswerProbabilities.OrderBy(x => x.Question.Id).ToList();

        var log = "XXX:" +
                  settings.DebugAnswerProbabilities.OrderBy(x => x.Question.Id)
                      .Select(x => x.Question.Id + ": " + x.CalculatedProbability)
                      .ToList()
                      .Aggregate((a, b) => a + " | " + b);

        Logg.r().Information(log);

        return true;
    }

    /// <summary>
    /// Only for testing
    /// </summary>
    public static void T_AddFinalBoost(
        Date date,
        TrainingPlanSettings settings,
        List<TrainingDate> learningDates)
    {
        AddFinalBoost(
            date,
            settings,
            learningDates);
    }

    public static void RemoveDatesIncludingAnswers(List<TrainingDate> trainingDates, List<TrainingDate> collidingDates, List<AnswerProbability> answerProbabilities)
    {
        collidingDates.ForEach(d => trainingDates.Remove(d));

        foreach (var answerProb in answerProbabilities)
        {
            var historiesToRemove = answerProb.History.Where(a => collidingDates.Any(d => d == a.TrainingDate));
            answerProb.History = answerProb.History.Except(historiesToRemove).ToList();
        }
    }

    private static void AddFinalBoost(
        Date date,
        TrainingPlanSettings settings,
        List<TrainingDate> learningDates)
    {
        var timeToBeFinished = date.DateTime.AddHours(settings.NumberOfHoursLastTrainingShouldEndBeforeDate);

        var estimatedTimeNeeded = new TimeSpan(0, 0, seconds: date.AllQuestions().Sum(q => q.TimeToLearnInSeconds()));

        var boostingDateProposal = 
            DateTimeUtils.RoundUp(timeToBeFinished, TimeSpan.FromMinutes(IntervalInMinutes))
                .Subtract(estimatedTimeNeeded);

        if(boostingDateProposal <= DateTimeX.Now())
            return;

        while (boostingDateProposal > DateTimeX.Now())
        {
            if (settings.IsInSnoozePeriod(boostingDateProposal))
            {
                boostingDateProposal = boostingDateProposal.AddMinutes(-IntervalInMinutes);
                continue;
            }

            break;
        }

        var collidingDates = learningDates
            .Where(d => boostingDateProposal.Subtract(d.DateTime).TotalMinutes < settings.SpacingBetweenSessionsInMinutes)
            .ToList();

        RemoveDatesIncludingAnswers(learningDates, collidingDates, settings.DebugAnswerProbabilities);

        var boostingDate = new TrainingDate();

        var precedingLearningDate = learningDates.OrderBy(d => d.DateTime).LastOrDefault();

    }

    private static List<AnswerProbability> ReCalcAllAnswerProbablities(DateTime dateTime, List<AnswerProbability> answerProbabilities)
    {
        var forgettingCurve = new ProbabilityCalc_Curve_HalfLife_24h(); 

        foreach (var answerProbability in answerProbabilities)
        {
            if (QuestionsToTrackIds.Contains(answerProbability.Question.Id))
            {
                Logg.r().Information("TrainingPlanCreator: Question " + answerProbability.Question.Id + "DateTime: " + dateTime.ToString("s") + ", oldProb: " + answerProbability.CalculatedProbability);
            }

            var newProbability = forgettingCurve.Run(
                answerProbability.History.Select(x => x.Answer).ToList(),
                answerProbability.Question,
                answerProbability.User,
                (int) (dateTime - answerProbability.CalculatedAt).TotalMinutes,
                answerProbability.CalculatedProbability
            );

            answerProbability.CalculatedProbability = newProbability;
            answerProbability.CalculatedAt = dateTime;

            if (QuestionsToTrackIds.Contains(answerProbability.Question.Id))
            {
                Logg.r().Information("TrainingPlanCreator: Question " + answerProbability.Question.Id + "DateTime: " + dateTime.ToString("s") + ", newProb: " + answerProbability.CalculatedProbability);
            }
        }

        return answerProbabilities;
    }
}