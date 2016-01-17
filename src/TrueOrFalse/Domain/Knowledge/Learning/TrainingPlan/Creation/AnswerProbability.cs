using System;
using System.Collections.Generic;
using System.Linq;

public class AnswerProbability
{
    public User User;
    public TimeSpan Offset;
    public Question Question;

    public int CalculatedProbability;
    public DateTime CalculatedAt;

    public IList<Answer> History;

    public void SetProbability(int value, DateTime dateTime)
    {
        CalculatedProbability = value;
        CalculatedAt = dateTime;
    }
}

public static class AnswerProbabilityListExts
{
    public static AnswerProbability By(this IList<AnswerProbability> answerProbabilities, int questionId)
    {
        return answerProbabilities.First(x => x.Question.Id == questionId);
    }
}