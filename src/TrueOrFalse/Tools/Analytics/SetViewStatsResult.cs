using System;

public class SetViewStatsResult
{
    private DateTime _goLive = new DateTime(2016, 10, 11);
    private DateTime _recordedSetViews = new DateTime(2017,2,17);
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

    public double SetDetailViewsDailyAvg => SetDetailViewsTotal / (DateTime.Now - (Created < _recordedSetViews ? _goLive : Created)).TotalDays
    public double QuestionViewsDailyAvg => QuestionsViewsTotal / (DateTime.Now - (Created < _goLive ? _goLive : Created)).TotalDays;
    
}
