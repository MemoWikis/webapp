public class AlgoTesterSummary
{
    public AlgoInfo Algo;

    public int TestCount;

    public int SuccessCount;
    public decimal SuccessRate;

    public decimal AvgDistance;
    public int AvgDistanceEXP2;

    public AlgoTesterSummary(AlgoSummary summary)
    {
        TestCount = (int)summary.TestCount;
        SuccessCount = (int)summary.SuccessCount;
        SuccessRate = summary.SuccessRate;
        AvgDistance = summary.AvgDistance;
        AvgDistanceEXP2 = (int)summary.AvgDistanceWeighted;
    }
}