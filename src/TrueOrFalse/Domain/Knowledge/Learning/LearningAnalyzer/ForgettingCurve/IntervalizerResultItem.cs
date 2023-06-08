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
}

