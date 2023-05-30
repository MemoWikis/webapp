using System;
using System.Collections.Generic;
using System.Linq;

public class ForgettingCurveLoader
{
    public static ForgettingCurve Get(
        IList<Answer> answers,
        ForgettingCurveInterval interval,
        int maxIntervalCount = 30)
    {
        var answerPairs = AnswerPairFromHistoryRows.Get(answers);
        var listOfExaminedAnswerObjects = answerPairs.OrderBy(x => x.TimePassedInSeconds).ToList();
        return Intervalizer.Run(listOfExaminedAnswerObjects, interval.ToTimespan(), maxIntervalCount);
    }

    public static ForgettingCurve GetForAll(ForgettingCurveInterval interval, int maxIntervalCount = 50)
    {
        return Get(Sl.R<AnswerRepo>().GetAll(), interval, maxIntervalCount);
    }
}

public enum ForgettingCurveInterval
{
    Minutes,
    Hours,
    Days,
    Week,
    Month /*Logarithmic*/
}

public static class ForgettingCurveIntervalExt
{
    public static string InGerman(this ForgettingCurveInterval interval)
    {
        switch (interval)
        {
            case ForgettingCurveInterval.Minutes: return "Minute";
            case ForgettingCurveInterval.Hours: return "Stunde";
            case ForgettingCurveInterval.Days: return "Tag";
            case ForgettingCurveInterval.Week: return "Woche";
            case ForgettingCurveInterval.Month: return "Monate";
        }

        throw new Exception("not implemented interval");
    }

    public static ForgettingCurveInterval ToForgettingCurveInterval(this string interval)
    {
        return (ForgettingCurveInterval)Enum.Parse(typeof(ForgettingCurveInterval), interval);
    }

    public static TimeSpan ToTimespan(this ForgettingCurveInterval interval)
    {
        switch (interval)
        {
            case ForgettingCurveInterval.Minutes: return new TimeSpan(0, 1, 0);
            case ForgettingCurveInterval.Hours: return new TimeSpan(1, 0, 0);
            case ForgettingCurveInterval.Days: return new TimeSpan(1, 0, 0, 0);
            case ForgettingCurveInterval.Week: return new TimeSpan(7, 0, 0, 0);
            case ForgettingCurveInterval.Month: return new TimeSpan(30, 0, 0, 0);
        }

        throw new Exception("not implemented interval");
    }
}