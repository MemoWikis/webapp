using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("NumberOfPairs: {NumberOfPairs}, Length: {TimeIntervalLength}")]
public class IntervalizerResultItem
{
    public TimeSpan TimeIntervalLength;
    public int IndexOfInterval;
    public List<Tuple<Answer, Answer>> PairsOfExaminedAndNextAnswer = new List<Tuple<Answer, Answer>>();
    public int NumberOfPairs;
    public double ProportionAnsweredCorrect;
    public TimeSpan TimePassedLowerBound;
    public TimeSpan TimePassedUpperBound;

    public IntervalizerResultItem(TimeSpan timeIntervalLength, int index)
    {
        TimeIntervalLength = timeIntervalLength;
        IndexOfInterval = index;
        TimePassedLowerBound = TimeSpan.FromSeconds(TimeIntervalLength.TotalSeconds * index);
        TimePassedUpperBound = TimeSpan.FromSeconds(TimeIntervalLength.TotalSeconds * (index + 1) - 1);
        NumberOfPairs = 0;
    }

    public void AddPair(Answer examinedAnswer, Answer nextAnswer)
    {
        PairsOfExaminedAndNextAnswer.Add(new Tuple<Answer, Answer>(examinedAnswer, nextAnswer));
        NumberOfPairs = PairsOfExaminedAndNextAnswer.Count;
        ProportionAnsweredCorrect = PairsOfExaminedAndNextAnswer.Count(p => p.Item2.AnsweredCorrectly()) / (double)NumberOfPairs;
    }

    public int TimePassedUpperBoundNumeric()
    {
        if (TimeIntervalLength.Minutes >= 0 && TimeIntervalLength.Hours <= 0) return (int)TimePassedUpperBound.TotalMinutes;
        if (TimeIntervalLength.Hours >= 0 && TimeIntervalLength.Days <= 0) return (int)TimePassedUpperBound.TotalHours;
        if (TimeIntervalLength.Hours >= 0 && TimeIntervalLength.Days <= 0)
            return (int)TimePassedUpperBound.TotalHours;

        return -1;
    }

    public double NumberOfInterval(ForgettingCurveInterval interval)
    {
        switch (interval)
        {
            case ForgettingCurveInterval.Minutes: return Math.Round(TimePassedUpperBound.TotalMinutes, 0);
            case ForgettingCurveInterval.Hours: return Math.Round(TimePassedUpperBound.TotalHours, 0);
            case ForgettingCurveInterval.Days: return Math.Round(TimePassedUpperBound.TotalDays, 0);
            case ForgettingCurveInterval.Week: return Math.Round(TimePassedUpperBound.TotalDays / 7, 0);
            case ForgettingCurveInterval.Month: return Math.Round(TimePassedUpperBound.TotalDays / 30, 0);
        }

        throw new Exception("not implemented type");
    }
}

