using System;

public class AlgoTesterSummary
{
    public AlgoInfo Algo;

    public int TestCount;

    public string FeatureName;
    public string FeatureGroup;

    public int SuccessCount;
    public decimal SuccessRate;
    public decimal SuccessRateInPercent;

    public decimal AvgDistance;
    public int AvgDistanceEXP2;

    public AlgoTesterSummary(AlgoSummary summary)
    {
        TestCount = (int)summary.TestCount;

        FeatureName = summary.FeatureName;
        FeatureGroup = summary.FeatureGroup;

        SuccessCount = (int)summary.SuccessCount;
        SuccessRate = summary.SuccessRate;
        AvgDistance = summary.AvgDistance;
        AvgDistanceEXP2 = (int)summary.AvgDistanceWeighted;
        SuccessRateInPercent = Math.Round(SuccessRate * 100, 0);
    }
}