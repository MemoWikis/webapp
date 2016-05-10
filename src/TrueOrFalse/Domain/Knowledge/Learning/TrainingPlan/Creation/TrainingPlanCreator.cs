using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        //if (date.AllQuestions().Count <= settings.QuestionsPerDate_Minimum)
        //    settings.QuestionsPerDate_Minimum = Math.Max(1, date.AllQuestions().Count);
        if (date.AllQuestions().Count < settings.QuestionsPerDate_Minimum)
            settings.QuestionsPerDate_Minimum = date.AllQuestions().Count;

        var answerRepo = Sl.R<AnswerRepo>();

        var answerProbabilities = 
            date
                .AllQuestions()
                .Select(q => 
                    new AnswerProbability
                    {
                        User = date.User,
                        Question = q,
                        CalculatedProbability = 90 /*ASANA: Übungsplan Startwert dynamisieren..*/,
                        CalculatedAt = DateTimeX.Now(),
                        History = answerRepo.GetByQuestion(q.Id, date.User.Id)
                    })
                .ToList();

        trainingPlan.Dates = GetDates(date, settings, answerProbabilities); 

        return trainingPlan;
    }

    private static IList<TrainingDate> GetDates(
        Date date, 
        TrainingPlanSettings settings, 
        List<AnswerProbability> answerProbabilities)
    {
        //var nextDateProposal = RoundUp(DateTimeX.Now(), TimeSpan.FromMinutes(15));
        var nextDateProposal = DateTimeX.Now();

        var learningDates = new List<TrainingDate>();

        while (nextDateProposal < date.DateTime)
        {
            nextDateProposal = nextDateProposal.AddMinutes(IntervalInMinutes);

            if (settings.IsInSnoozePeriod(nextDateProposal))
                continue;

            if(TryAddDate(settings, nextDateProposal, answerProbabilities, learningDates))
                nextDateProposal = nextDateProposal.AddMinutes(settings.SpacingBetweenSessionsInMinutes
                    //- IntervalInMinutes
                    );
        }

        return learningDates;
    }

    private static DateTime RoundUp(DateTime dateTime, TimeSpan roundToNextFull)
    {
        //http://stackoverflow.com/a/7029464
        return new DateTime(((dateTime.Ticks + roundToNextFull.Ticks - 1) / roundToNextFull.Ticks) * roundToNextFull.Ticks);
    }

    private static bool TryAddDate(
        TrainingPlanSettings settings,
        DateTime proposedDateTime,
        List<AnswerProbability> answerProbabilities,
        List<TrainingDate> learningDates)
    {
        var answerProbabilites = 
            ReCalcAllAnswerProbablities(proposedDateTime, answerProbabilities)
            .OrderBy(x => x.CalculatedProbability)
            .ThenBy(x => x.History.Count);

        var belowThresholdCount = answerProbabilites.Count(x => x.CalculatedProbability < settings.AnswerProbabilityThreshold);

        if (answerProbabilities.Count(x => x.CalculatedProbability < 15) > 0)
            Debugger.Break();

        if((proposedDateTime.Hour % 4) == 0 && proposedDateTime.Minute < 15)
            answerProbabilities.Log();

        if (belowThresholdCount < settings.QuestionsPerDate_Minimum)
            return false;

        var trainingDate = new TrainingDate{DateTime = proposedDateTime};
        learningDates.Add(trainingDate);

        trainingDate.AllQuestions =
            answerProbabilites
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

            answerProbabilities
                .By(trainingQuestion.Question.Id)
                .SetProbability(trainingQuestion.ProbAfter, trainingDate.DateTime);

            if (settings.QuestionsPerDate_IdealAmount < i + 2)
                break;
        }

        var probs = answerProbabilities.OrderBy(x => x.Question.Id).ToList();

        var log = "XXX:" +
                  answerProbabilities.OrderBy(x => x.Question.Id)
                      .Select(x => x.Question.Id + ": " + x.CalculatedProbability)
                      .ToList()
                      .Aggregate((a, b) => a + " | " + b);

        Logg.r().Information(log);

        return true;
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
                answerProbability.History,
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