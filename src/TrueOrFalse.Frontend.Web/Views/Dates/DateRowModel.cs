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

    public bool IsPast;
    public bool IsNetworkDate;

    public DateRowModel(Date date, bool isNetworkDate = false)
    {
        Date = date;

        var allQuestions = date.AllQuestions();
        AmountQuestions = allQuestions.Count;

        var summary = R<KnowledgeSummaryLoader>().Run(UserId, allQuestions.GetIds(), onlyValuated: false);

        KnowledgeSecure = summary.Secure;
        KnowledgeUnknown = summary.Unknown;
        KnowledgeWeak = summary.Weak;

        var remaining = date.DateTime - DateTime.Now;
        RemainingDays = Math.Abs(Convert.ToInt32(remaining.TotalDays));
        RemainingMinutes = Math.Abs(Convert.ToInt32(remaining.TotalMinutes));

        ShowMinutesLeft = RemainingDays == 0;

        IsPast = remaining.TotalSeconds < 0;
        IsNetworkDate = isNetworkDate;
    }
}