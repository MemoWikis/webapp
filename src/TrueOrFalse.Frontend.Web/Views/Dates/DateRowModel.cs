using System;

public class DateRowModel : BaseModel
{
    public Date Date;

    public int KnowledgeUnknown;
    public int KnowledgeWeak;
    public int KnowledgeSecure;

    public int AmountQuestions;

    public bool ShowMinutesLeft;

    public int RemainingDays;
    public int RemainingMinutes;
    
    public DateRowModel(Date date)
    {
        Date = date;

        var allQuestions = date.AllQuestions();
        AmountQuestions = allQuestions.Count;

        var summary = R<KnowledgeSummaryLoader>().Run(UserId, allQuestions.GetIds());

        KnowledgeSecure = summary.Secure;
        KnowledgeUnknown = summary.Unknown;
        KnowledgeWeak = summary.Weak;

        var remaining = date.DateTime - DateTime.Now;
        RemainingDays = Convert.ToInt32(remaining.TotalDays);
        RemainingMinutes = Convert.ToInt32(remaining.TotalMinutes);

        ShowMinutesLeft = RemainingDays == 0;
    }
}
