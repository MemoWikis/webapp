using System;

public class TrainingDateModel
{
    public DateTime DateTime;
    public int QuestionCount;

    public Date Date;

    private TimeSpanLabel _timeSpanLabel;

    public int Minutes
    {
        get { return (int) Math.Ceiling(30/*seconds per question*/ * QuestionCount / 60d); }
    }

    public TimeSpanLabel TimeSpanLabel
    {
        get { return _timeSpanLabel ?? (_timeSpanLabel = new TimeSpanLabel(DateTime.Now - DateTime)); }
    }

    public string GetTitle()
    {
        return Date != null ? 
            Date.GetTitle() : 
            "Kontinuierliches lernen";
    }

    public bool IsContinousLearning{ get { return Date == null; } }

    public KnowledgeSummary SummaryBefore;
    public KnowledgeSummary SummaryAfter;
}