using System;

[Serializable]
public class LeitnerAnswer : IAnswered
{
    public int DayAnswered;
    public bool WasCorrect;
    public int ProbabilityBefore;

    public bool AnsweredCorrectly() => WasCorrect;

    private double _answerOffsetInMinutes = 0;
    public double GetAnswerOffsetInMinutes() => _answerOffsetInMinutes;

    public void SetResultFor_GetAnswerOffsetInMinutes(int currentDay)
    {
        _answerOffsetInMinutes = (currentDay - DayAnswered) * 24 * 60;
    }
}