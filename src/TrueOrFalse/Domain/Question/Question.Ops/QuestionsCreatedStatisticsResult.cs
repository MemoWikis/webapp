using System;

public class QuestionsCreatedStatisticsResult
{
    public DateTime DateTime;

    public int CountByMemucho;
    public int CountByOthers;
    public int TotalCount { get { return CountByMemucho + CountByOthers; } }
}
