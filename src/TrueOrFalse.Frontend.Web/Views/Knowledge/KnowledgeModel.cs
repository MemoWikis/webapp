using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class KnowledgeModel : BaseModel
{
    public string UserName
    {
        get
        {
            if (_sessionUser.User == null)
                return "Unbekannte(r)";
            return _sessionUser.User.Name;
        }
    }

    public GetAnswerStatsInPeriodResult AnswersThisWeek;
    public GetAnswerStatsInPeriodResult AnswersThisMonth;
    public GetAnswerStatsInPeriodResult AnswersThisYear;
    public GetAnswerStatsInPeriodResult AnswersLastMonth;
    public GetAnswerStatsInPeriodResult AnswersLastWeek;
    public GetAnswerStatsInPeriodResult AnswersLastYear;
    public GetAnswerStatsInPeriodResult AnswersEver;

    public int QuestionsCount;
    public int SetCount;

    public KnowledgeSummary KnowledgeSummary;

    public KnowledgeModel()
    {
        var sp = Stopwatch.StartNew();

        if (IsLoggedIn)
        {
            Loggly.Send("Dashboard-Probability-Start: " + sp.Elapsed, LogglyCategories.Performance);
            R<ProbabilityForUserUpdate>().Run(UserId);
            Loggly.Send("Dashboard-Probability-Stop: " + sp.Elapsed, LogglyCategories.Performance);
        }

        QuestionsCount = R<GetWishQuestionCountCached>().Run(UserId);
        SetCount = R<GetWishSetCount>().Run(UserId);

        var getAnswerStatsInPeriod = Resolve<GetAnswerStatsInPeriod>();
        AnswersThisWeek = getAnswerStatsInPeriod.RunForThisWeek(UserId);
        AnswersThisMonth = getAnswerStatsInPeriod.RunForThisMonth(UserId);
        AnswersThisYear = getAnswerStatsInPeriod.RunForThisYear(UserId);
        AnswersLastWeek = getAnswerStatsInPeriod.RunForLastWeek(UserId);
        AnswersLastMonth = getAnswerStatsInPeriod.RunForLastMonth(UserId);
        AnswersLastYear = getAnswerStatsInPeriod.RunForLastYear(UserId);
        AnswersEver = getAnswerStatsInPeriod.Run(UserId);

        KnowledgeSummary = R<KnowledgeSummaryLoader>().Run(UserId);
    }
}