using System;

public class ProbabilityCalc_Curve
{
    public int Run(Question question, int offsetInSeconds)
    {
        var offsetInMinutes = offsetInSeconds*60;

        if (question.IsEasyQuestion())
            return GetProbability(offsetInMinutes, );

        return -1;
    }

    public double GetProbability(double minutes, int coefficient)
    {
        return Math.Pow(100*Math.E, -(1d/coefficient*minutes));
    }
}

