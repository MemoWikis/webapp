using System;
using System.Collections.Generic;

public class AnswerPair
{
    public Answer NextAnswer;
    public Answer ExaminedAnswer;
    public List<Answer> AnswerHistoryRow;
    /// <summary>
    /// Time span between examined and next answer
    /// </summary>
    public TimeSpan TimePassed;
    public double TimePassedInSeconds;

    public AnswerPair(List<Answer> answerHistoryRow)
    {
        if (answerHistoryRow.Count <= 1)
            return;

        AnswerHistoryRow = answerHistoryRow;
        NextAnswer = answerHistoryRow[answerHistoryRow.Count - 1];
        ExaminedAnswer = answerHistoryRow[answerHistoryRow.Count - 2];
        TimePassed = NextAnswer.DateCreated.Subtract(ExaminedAnswer.DateCreated);
        TimePassedInSeconds = TimePassed.TotalSeconds;
    }
}
