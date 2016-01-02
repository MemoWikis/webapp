using System;

public static class ProbabilityCalc_Curve
{
    public static int GetProbability(double minutes, int stability, int startValue = 100)
    {
        return (int) Math.Round(Math.Pow(startValue*Math.E, -(1d * minutes / stability)) * 100, 0);
    }
}

/// <summary>After 12h 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_12h{
    public int Run(Question question, int offsetInMinutes, int startValue){
        return  ProbabilityCalc_Curve.GetProbability(offsetInMinutes, stability: 5055, startValue: startValue);
    }
}

/// <summary>After 24h 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_24h{
    public int Run(Question question, int offsetInMinutes, int startValue){
        return ProbabilityCalc_Curve.GetProbability(offsetInMinutes, stability: 10109, startValue: startValue);
    }
}

/// <summary>After 7days 50% probability</summary>
public class ProbabilityCalc_Curve_HalfLife_7days{
    public int Run(Question question, int offsetInMinutes, int startValue){
        return ProbabilityCalc_Curve.GetProbability(offsetInMinutes, stability: 70758, startValue: startValue);
    }
}