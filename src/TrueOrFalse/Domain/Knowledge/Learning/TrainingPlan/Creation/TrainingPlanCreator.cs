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

        var probUpdateValRepo = Sl.R<ProbabilityUpdate_Valuation>();

        foreach (var question in date.AllQuestions())
        {
            probUpdateValRepo.Run(question.Id, date.User.Id);
        }

        var answerRepo = Sl.R<AnswerRepo>();
        var questionValuationRepo = Sl.R<QuestionValuationRepo>();

        var answerProbabilities = 
            date
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

        trainingPlan.Dates = GetDates(date, settings, answerProbabilities);


        return trainingPlan;
    }

    private static IList<TrainingDate> GetDates(
        Date date, 
        TrainingPlanSettings settings, 
        List<AnswerProbability> answerProbabilities)
    {
        var nextDateProposal = DateTimeUtils.RoundUp(DateTimeX.Now(), TimeSpan.FromMinutes(15));

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

        AddOverlearning(date, settings, answerProbabilities, learningDates);

        return learningDates;
    }

    private static bool TryAddDate(
        TrainingPlanSettings settings,
        DateTime proposedDateTime,
        List<AnswerProbability> answerProbabilities,
        List<TrainingDate> learningDates)
    {
        var newAnswerProbabilities = 
            ReCalcAllAnswerProbablities(proposedDateTime, answerProbabilities)
            .OrderBy(x => x.CalculatedProbability)
            .ThenBy(x => x.History.Count);

        var belowThresholdCount = newAnswerProbabilities.Count(x => x.CalculatedProbability < settings.AnswerProbabilityThreshold);

        if (answerProbabilities.Count(x => x.History.Count > 0 && x.CalculatedProbability < 15) > 0)
            Debugger.Break();

        //if((proposedDateTime.Hour % 4) == 0 && proposedDateTime.Minute < 15)
        //    answerProbabilities.Log();

        if (belowThresholdCount < settings.QuestionsPerDate_Minimum)
            return false;

        var trainingDate = new TrainingDate{DateTime = proposedDateTime};
        learningDates.Add(trainingDate);

        trainingDate.AllQuestions =
            newAnswerProbabilities
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
                .AddAnswerAndSetProbability(trainingQuestion.ProbAfter, trainingDate.DateTime, trainingDate);

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

    /// <summary>
    /// Only for testing
    /// </summary>
    public static void T_AddOverLearning(
        Date date,
        TrainingPlanSettings settings,
        List<AnswerProbability> answerProbabilities,
        List<TrainingDate> learningDates)
    {
        AddOverlearning(
            date,
            settings,
            answerProbabilities,
            learningDates);
    }

    private static void AddOverlearning(
        Date date,
        TrainingPlanSettings settings,
        List<AnswerProbability> answerProbabilities,
        List<TrainingDate> learningDates)
    {
        var overlearningDateProposal = date.DateTime.AddHours(-2.75);

        while (overlearningDateProposal > DateTimeX.Now())
        {
            overlearningDateProposal = overlearningDateProposal.AddMinutes(-IntervalInMinutes);

            if (settings.IsInSnoozePeriod(overlearningDateProposal))
                continue;

            break;
        }

        var collidingDates = learningDates
            .Where(d => overlearningDateProposal.Subtract(d.DateTime).TotalMinutes < settings.SpacingBetweenSessionsInMinutes)
            //.Select(d => d.DateTime)
            .ToList();

        //learningDates.RemoveAll(d => collidingDates.Contains(d.DateTime));
        collidingDates.ForEach(d => learningDates.Remove(d));

        foreach (var answerProb in answerProbabilities)
        {
            var newHistory = answerProb.History.ToList();
            //var subset = newHistory.Where(a => collidingDates.Any(d => d.DateTime == a.DateCreated)).ToList();
            //newHistory.RemoveAll(a => collidingDates.Any(d => d.DateTime == a.DateCreated));
            answerProb.History = newHistory;
        }
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