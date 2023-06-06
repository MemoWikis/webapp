public class GetStreaksDaysResult
{
    public DateTime LongestStart;
    public DateTime LongestEnd;
    public int LongestLength = 0;

    public DateTime LastStart;
    public DateTime LastEnd;
    public int LastLength = 0;

    public bool LastIsLongest => LongestStart == LastStart;

    public bool HasStreakInRecentPeriod => RecentPeriodSLongestLength == 0;
    public DateTime RecentPeriodSince;
    public DateTime RecentPeriodSLongestStart;
    public DateTime RecentPeriodSLongestEnd;
    public int RecentPeriodSLongestLength = 0;

    public int TotalLearningDays = 0;
}