using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimeIntervalWithAnswers
{
    public TimeSpan TimeIntervalLength;
    public int IndexOfInterval;
    public List<Tuple<AnswerHistory, AnswerHistory>> PairsOfExaminedAndNextAnswer = new List<Tuple<AnswerHistory, AnswerHistory>>();
    public int NumberOfPairs;
    public double ProportionAnsweredCorrect;
    public TimeSpan TimePassedLowerBound;
    public TimeSpan TimePassedUpperBound;

    public TimeIntervalWithAnswers(TimeSpan timeIntervalLength, int index)
    {
        TimeIntervalLength = timeIntervalLength;
        IndexOfInterval = index;
        TimePassedLowerBound = TimeSpan.FromSeconds(TimeIntervalLength.TotalSeconds * index);
        TimePassedUpperBound = TimeSpan.FromSeconds(TimeIntervalLength.TotalSeconds * (index + 1) - 1);
        NumberOfPairs = 0;
    }

    public void AddPair(AnswerHistory examinedAnswer, AnswerHistory nextAnswer)
    {
        PairsOfExaminedAndNextAnswer.Add(new Tuple<AnswerHistory, AnswerHistory>(examinedAnswer, nextAnswer));
        NumberOfPairs = PairsOfExaminedAndNextAnswer.Count;
        ProportionAnsweredCorrect = PairsOfExaminedAndNextAnswer.Count(p => p.Item2.AnsweredCorrectly()) / (double)NumberOfPairs;
    }
}

