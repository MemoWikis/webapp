using System;

public class QuestionsCreatedPerDayResult
{
    public DateTime DateTime;

    public int CountByMemucho;
    public int CountByOthers;
    public int TotalCount { get { return CountByMemucho + CountByOthers; } }
}
