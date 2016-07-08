using System;

[Serializable]
public class LeitnerAnswer : IAnswered
{
    public int Day;
    public bool WasCorrect;
    public int ProbabilityBefore;

    public bool AnsweredCorrectly() => WasCorrect;

    private double _answerOffsetInMinutes = 0;
    public double GetAnswerOffsetInMinutes() => _answerOffsetInMinutes;

    public void SetResultFor_GetAnswerOffsetInMinutes(double minutes) => _answerOffsetInMinutes = minutes;
}