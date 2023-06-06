using System.Collections.Generic;

public class AnswerPair
{
    public Answer NextAnswer;
    public Answer ExaminedAnswer;
    public List<Answer> AnswerRow;
    /// <summary>
    /// Time span between examined and next answer
    /// </summary>
    public TimeSpan TimePassed;
    public double TimePassedInSeconds;

    public AnswerPair(List<Answer> answerRow)
    {
        if (answerRow.Count <= 1)
            return;

        AnswerRow = answerRow;
        NextAnswer = answerRow[answerRow.Count - 1];
        ExaminedAnswer = answerRow[answerRow.Count - 2];
        TimePassed = NextAnswer.DateCreated.Subtract(ExaminedAnswer.DateCreated);
        TimePassedInSeconds = TimePassed.TotalSeconds;
    }
}
