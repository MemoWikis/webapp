using System.Collections.Generic;
using NHibernate.Util;

public class AnswerPatternInfo
{
    public string Name;
    public int MatchedAnswersCount;
    public IList<Answer> Matches;

    public IList<Answer> NextAnswers;

    public double NextAnswerAvgCorrect()
    {
        if (!NextAnswers.Any())
            return 0;

        return NextAnswers.AverageCorrectness();
    }
}
