using System;

public class DateRowModel : BaseModel
{
    public Date Date;

    public int KnowledgeNotLearned;
    public int KnowledgeNeedsLearning;
    public int KnowledgeNeedsConsolidation;
    public int KnowledgeSolid;

    public int AmountQuestions;

    public bool ShowMinutesLeft;
    public bool ShowHoursLeft;
    
    public TimeSpanLabel RemainingLabel;

    public bool IsPast;
    public bool IsNetworkDate;

    public DateRowModel(Date date, bool isNetworkDate = false)
    {
        Date = date;

        var allQuestions = date.AllQuestions();
        AmountQuestions = allQuestions.Count;

        var summary = R<KnowledgeSummaryLoader>().Run(UserId, allQuestions.GetIds(), onlyValuated: false);

        KnowledgeNotLearned = summary.NotLearned;
        KnowledgeNeedsLearning = summary.NeedsLearning;
        KnowledgeNeedsConsolidation = summary.NeedsConsolidation;
        KnowledgeSolid = summary.Solid;

        var remaining = date.Remaining();
        IsPast = remaining.TotalSeconds < 0;
        RemainingLabel = new TimeSpanLabel(remaining, IsPast);
        IsNetworkDate = isNetworkDate;
    }
}