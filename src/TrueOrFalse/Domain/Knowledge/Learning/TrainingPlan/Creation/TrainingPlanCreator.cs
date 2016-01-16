using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanCreator
{
    public static TrainingPlan Run(Date date, TrainingPlanSettings settings)
    {
        var learnPlan = new TrainingPlan();
        learnPlan.Date = date;
        learnPlan.Settings = settings;

        var answerProbabilities = 
            date
                .AllQuestions()
                .Select(x => 
                    new AnswerProbability
                    {
                        Question = x,
                        CalculatedProbability = 90 /*Do: get real number..*/,
                        CalculatedAt = DateTimeX.Now()
                    })
                .ToList();

        learnPlan.Dates = GetDates(date, settings, answerProbabilities);

        return learnPlan;
    }

    private static IList<TrainingDate> GetDates(
        Date date, 
        TrainingPlanSettings settings, 
        List<AnswerProbability> answerProbabilities)
    {
        var nextDateProposal = DateTimeX.Now();
        var learningDates = new List<TrainingDate>();

        while (nextDateProposal < date.DateTime)
        {
            nextDateProposal = nextDateProposal.AddMinutes(15);

            if (settings.IsInSnoozePeriod(nextDateProposal))
                continue;

            AddDate(settings, nextDateProposal, answerProbabilities, learningDates);
            nextDateProposal = nextDateProposal.AddMinutes(settings.SpacingBetweenSessionsInMinutes);
        }

        return learningDates;
    }

    private static void AddDate(
        TrainingPlanSettings settings,
        DateTime dateTime,
        List<AnswerProbability> answerProbabilities,
        List<TrainingDate> learningDates)
    {
        var answerProbabilites = 
            ReCalcAllAnswerProbablities(dateTime, answerProbabilities).
            OrderBy(x => x.CalculatedProbability);

        var applicableCount = answerProbabilites.Count(x => x.CalculatedProbability < 90);

        if (applicableCount >= settings.QuestionsPerDate_Minimum)
        {
            var trainingDate = new TrainingDate{DateTime = dateTime};
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

            for (int i = 0; i < applicableCount; i++)
            {
                var trainingQuestion = trainingDate.AllQuestions[i];
                trainingQuestion.IsInTraining = true;
                /*directly after training the probability is almost 100%!*/
                trainingQuestion.ProbAfter = 99;

                answerProbabilities
                    .By(trainingQuestion.Question.Id)
                    .SetProbability(trainingQuestion.ProbAfter, trainingDate.DateTime);

                if (settings.QuestionsPerDate_IdealAmount < i)
                    break;
            }
        }
    }

    private static List<AnswerProbability> ReCalcAllAnswerProbablities(DateTime dateTime, List<AnswerProbability> answerProbabilities)
    {
        var forgettingCurve = new ProbabilityCalc_Curve_HalfLife_24h(); 

        foreach (var answerProbability in answerProbabilities)
        {
             var newProbability = forgettingCurve.Run(
                answerProbability.Question, 
                (int) (dateTime - answerProbability.CalculatedAt).TotalMinutes,
                answerProbability.CalculatedProbability
            );

            answerProbability.CalculatedProbability = newProbability;
            answerProbability.CalculatedAt = dateTime;
        }

        return answerProbabilities;
    }
}