using System;

public static class ProbabilityCalc_Curve
{
    public static int GetProbability(double minutes, int coefficient)
    {
        return (int) Math.Round(Math.Pow(100*Math.E, -(1d/coefficient*minutes)), 0);
    }
}

/// <summary>After 12h 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_12h{
    public int Run(Question question, int offsetInMinutes){
        return  ProbabilityCalc_Curve.GetProbability(offsetInMinutes, coefficient: 5055);
    }
}

/// <summary>After 24h 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_24h{
    public int Run(Question question, int offsetInMinutes){
        return ProbabilityCalc_Curve.GetProbability(offsetInMinutes, coefficient: 10109);
    }
}

/// <summary>After 7days 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_7days{
    public int Run(Question question, int offsetInMinutes){
        return ProbabilityCalc_Curve.GetProbability(offsetInMinutes, coefficient: 70758);
    }
}