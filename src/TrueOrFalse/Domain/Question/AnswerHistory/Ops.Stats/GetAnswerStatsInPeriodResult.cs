public class GetAnswerStatsInPeriodResult
{
    public int TotalAnswers;
    public int TotalTrueAnswers;
    public int TotalFalseAnswers { get { return TotalAnswers - TotalTrueAnswers; } }

    public DateTime DateTime;
}