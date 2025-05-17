public class GetAnswerStatsInPeriodResult
{
    public int TotalAnswers;
    public int TotalTrueAnswers;
    public int TotalFalseAnswers => TotalAnswers - TotalTrueAnswers;

    public DateTime DateTime;
}