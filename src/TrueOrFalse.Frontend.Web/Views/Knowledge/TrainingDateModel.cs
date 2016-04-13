using System;

public class TrainingDateModel
{
    public int Id;
    public DateTime DateTime;
    public int QuestionCount;

    public Date Date;

    public int Minutes => (int) Math.Ceiling(30/*seconds per question*/ * QuestionCount / 60d);

    private TimeSpanLabel _timeSpanLabel;
    public TimeSpanLabel TimeSpanLabel => _timeSpanLabel ?? (_timeSpanLabel = new TimeSpanLabel(DateTime.Now - DateTime));

    public bool IsContinousLearning => Date == null;

    public KnowledgeSummary SummaryBefore;
    public KnowledgeSummary SummaryAfter;

    public string GetTitle()
    {
        return Date != null ?
            Date.GetTitle() :
            "Langzeitlernen";
    }
}