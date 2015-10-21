using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExaminedAnswerObject
{
    public AnswerHistory NextAnswer;
    public AnswerHistory ExaminedAnswer;
    public List<AnswerHistory> AnswerHistoryRow;
    /// <summary>
    /// Time span between examined and next answer
    /// </summary>
    public TimeSpan TimePassed;
    public double TimePassedInSeconds;

    public ExaminedAnswerObject(List<AnswerHistory> answerHistoryRow)
    {
        if (answerHistoryRow.Count <= 1) return;
        AnswerHistoryRow = answerHistoryRow;
        NextAnswer = answerHistoryRow[answerHistoryRow.Count - 1];
        ExaminedAnswer = answerHistoryRow[answerHistoryRow.Count - 2];
        TimePassed = NextAnswer.DateCreated.Subtract(ExaminedAnswer.DateCreated);
        TimePassedInSeconds = TimePassed.TotalSeconds;
    }
}
