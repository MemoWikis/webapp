using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using TrueOrFalse.Web;

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

    public IList<GetAnswerStatsInPeriodResult> Last30Days = new List<GetAnswerStatsInPeriodResult>();
    public bool HasLearnedInLast30Days;

    public int QuestionsCount;
    public int SetCount;

    public KnowledgeSummary KnowledgeSummary = new KnowledgeSummary();
    public GetStreaksDaysResult StreakDays = new GetStreaksDaysResult();

    public IList<Date> Dates = new List<Date>();
    public IList<Date> DatesInNetwork = new List<Date>();
    public IList<AnswerHistory> AnswerRecent = new List<AnswerHistory>();

    public User User = new User();
    public int ReputationRank;
    public int ReputationTotal;

    public UIMessage Message;

    public IList<UserActivity> NetworkActivities;
    public IList<string> NetworkActivitiesActionString;


    public KnowledgeModel(string emailKey = null)
    {
        if (!String.IsNullOrEmpty(emailKey))
            if (R<ValidateEmailConfirmationKey>().IsValid(emailKey))
                Message = new SuccessMessage("Deine E-Mail-Ad­res­se ist nun bestätigt.");

        if(HttpContext.Current.Request["passwordSet"] != null)
            Message = new SuccessMessage("Du hast dein Passwort aktualisiert.");

        if (!IsLoggedIn)
            return;

        QuestionsCount = R<GetWishQuestionCountCached>().Run(UserId);
        SetCount = R<GetWishSetCount>().Run(UserId);
        User = R<UserRepo>().GetById(UserId);

        var reputation = Resolve<ReputationCalc>().Run(User);
        ReputationRank = User.ReputationPos;
        ReputationTotal = reputation.TotalRepuation;

        var msg = new RecalcProbabilitiesMsg {UserId = UserId};
        Bus.Get().Publish(msg);

        var getAnswerStatsInPeriod = Resolve<GetAnswerStatsInPeriod>();

        Last30Days = getAnswerStatsInPeriod.GetLast30Days(UserId);
        HasLearnedInLast30Days = (Last30Days.Sum(d => d.TotalAnswers) > 0);

        KnowledgeSummary = R<KnowledgeSummaryLoader>().Run(UserId);

        //Dates = GetSampleDates.Run();
        Dates = R<DateRepo>().GetBy(UserId, true);
        DatesInNetwork = R<GetDatesInNetwork>().Run(UserId);


        AnswerRecent = R<GetLastAnswers>().Run(UserId, 5);
        StreakDays = R<GetStreaksDays>().Run(User);

        //GET NETWORK ACTIVITY
        NetworkActivities = R<UserActivityRepo>().GetByUser(User, 8);

    }

    private static string Plural(int amount, string pluralSuffix, string singularSuffix = "", string zeroSuffix = "")
    {
        if (amount > 1)
            return pluralSuffix;
        return amount == 0 ? zeroSuffix : singularSuffix;
        //todo: Ask Robert, why I cannot use HtmlExtensions.Plural here (because it's for Views... -> well, then why implemented in such a way?)
        //todo: Anyway: Move this function to some more accessible place.
    }

    public string ConvertTime(DateTime dateTime)
    {
        //todo: Ask Robert if implementating method here okay... -> should go somewhere widely accessible as static method, but where?
        var remaining = DateTime.Now - dateTime;
        var daysAgo = Math.Abs(Convert.ToInt32(remaining.TotalDays));
        var hoursAgo = Math.Abs(Convert.ToInt32(remaining.TotalHours));
        var minutesAgo = Math.Abs(Convert.ToInt32(remaining.TotalMinutes));

        if (daysAgo == 0 && hoursAgo == 0)
            return "Vor " + minutesAgo + " Minute" + Plural(minutesAgo, "n","","n");
        if (daysAgo == 0)
            return "Vor " + hoursAgo + " Stunde" + Plural(hoursAgo, "n");
        return "Vor " + daysAgo + "Tag" + Plural(daysAgo, "en");
    }
}