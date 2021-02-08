using System;
using System.Collections.Generic;
using System.Linq;

public static class ProbabilityCalc_Curve
{
    public static int GetProbability(double minutes, int stability, int startValue = 100)
    {
        if (minutes == 0 && stability == 0)
            return startValue;

        return (int) Math.Round(Math.Pow(Math.E, -(1d * minutes / stability)) * startValue, 0);
    }
}

/// <summary>After 12h 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_12h{
    public int Run(Question question, int offsetInMinutes, int startValue){
        return  ProbabilityCalc_Curve.GetProbability(offsetInMinutes, stability: 5055, startValue: startValue);
    }
}

/// <summary>After 24h 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_24h
{
    public const int Stability = 2048;

    public int Run(
        IList<Answer> previousAnswers,
        Question question,
        User user,
        int offsetInMinutes, 
        int startValue)
    {
        var stability = Stability + GetStabilityModificator(previousAnswers.ToList<IAnswered>());
        return ProbabilityCalc_Curve.GetProbability(offsetInMinutes, stability, startValue: startValue);
    }

    ///naive implementation!
    public static int GetStabilityModificator(IEnumerable<IAnswered> previousAnswers)
    {
        return previousAnswers.Sum(a =>
        {
            var offsetInMinutes = a.GetAnswerOffsetInMinutes();
            var probability = ProbabilityCalc_Curve.GetProbability(offsetInMinutes, Stability, startValue: 100);

            if (a.AnsweredCorrectly())
                return 100 * probability;
            else
                return 80 * probability;
        });
    }


}

/// <summary>After 7days 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_7days{
    public int Run(Question question, int offsetInMinutes, int startValue){
        return ProbabilityCalc_Curve.GetProbability(offsetInMinutes, stability: 70758, startValue: startValue);
    }
}