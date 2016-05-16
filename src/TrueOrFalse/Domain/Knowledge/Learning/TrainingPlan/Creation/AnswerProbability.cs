using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("%{CalculatedProbability} History.Count:{History.Count}")]
public class AnswerProbability
{
    public User User;
    public TimeSpan Offset;
    public Question Question;

    public int CalculatedProbability;
    public DateTime CalculatedAt;

    public IList<Answer> History;

    public void AddAnswerAndSetProbability(int value, DateTime dateTime, TrainingDate trainingDate)
    {
        History.Add(new Answer
        {
            AnswerredCorrectly = AnswerCorrectness.True,
            DateCreated = DateTimeX.Now()
        });
        CalculatedProbability = value;
        CalculatedAt = dateTime;
    }

    public string ToLogString()
    {
        return $"Q: {Question.Id} P: {CalculatedProbability} H: {History.Count} C:{CalculatedAt.ToString("d")}";
    }
}

public static class AnswerProbabilityListExts
{
    public static AnswerProbability By(this IList<AnswerProbability> answerProbabilities, int questionId)
    {
        return answerProbabilities.First(x => x.Question.Id == questionId);
    }

    public static void Log(this IList<AnswerProbability> answerProbabilities)
    {
        Logg.r().Information(answerProbabilities.Select(x => x.ToLogString()).Aggregate((a,b) => a + " " + b + Environment.NewLine));
    }
}