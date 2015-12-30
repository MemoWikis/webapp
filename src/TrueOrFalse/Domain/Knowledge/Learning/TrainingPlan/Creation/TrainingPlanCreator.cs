using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanCreator
{
    public static TrainingPlan Run(Date date, TrainingPlanSettings settings)
    {
        var learnPlan = new TrainingPlan();

        learnPlan.Date = date;
        learnPlan.Dates = GetDates(date, settings);

        return learnPlan;
    }

    private static IList<TrainingDate> GetDates(Date date, TrainingPlanSettings settings)
    {
        var nextDateProposal = DateTime.Now;
        var learningDates = new List<TrainingDate>();
        var answerProbabilities = date.AllQuestions().Select(x => new AnswerProbability{ Question = x }).ToList();

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
            CalcAllAnswerProbablities(dateTime, answerProbabilities).
            OrderByDescending(x => x.Probability);

        var applicableCount = answerProbabilites.Count(x => x.Probability < 90);

        if (applicableCount >= settings.QuestionsPerDate_Minimum)
        {
            var trainingDate = new TrainingDate();
            learningDates.Add(trainingDate);

            trainingDate.AllQuestions =
                answerProbabilites
                    .Select(x => new TrainingQuestion
                    {
                        Question = x.Question,
                        ProbBefore = x.Probability,
                        ProbAfter = x.Probability
                    })
                    .ToList();

            for (int i = 0; i < applicableCount; i++)
            {
                trainingDate.AllQuestions[i].IsInTraining = true;
                trainingDate.AllQuestions[i].ProbAfter = 98;
            }
        }
    }

    private static List<AnswerProbability> CalcAllAnswerProbablities(DateTime dateTime, List<AnswerProbability> answerProbabilities)
    {
        return answerProbabilities;
    }
}