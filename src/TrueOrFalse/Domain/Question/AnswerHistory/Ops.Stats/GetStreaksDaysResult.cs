using System;

public class GetStreaksDaysResult
{
    public DateTime LongestStart;
    public DateTime LongestEnd;
    public int LongestLength = 0;

    public DateTime LastStart;
    public DateTime LastEnd;
    public int LastLength = 0;

    public int TotalLearningDays = 0;
}