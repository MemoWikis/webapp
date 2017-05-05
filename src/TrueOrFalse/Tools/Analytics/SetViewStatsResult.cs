using System;

public class SetViewStatsResult
{
    //public DateTime LastUpdated; //needed when results are stored in table
    public int SetId;
    public string SetName;
    public DateTime Created;

    public int SetDetailViewsTotal;
    public int QuestionsViewsTotal;
    public int QuestionsAnswersTotal;
    public int LearningSessionsTotal;
    public int DatesTotal;

    public int SetDetailViewsLast7Days;
    public int SetDetailViewsLast30Days;
    public int SetDetailViewsPrec30Days;

    public int QuestionsViewsLast7Days;
    public int QuestionsViewsLast30Days;
    public int QuestionsViewsPrec30Days;

    public double SetDetailViewsWeeklyAvg => SetDetailViewsTotal / (DateTime.Now - Created).TotalDays / 7;
}
