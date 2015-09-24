using System;
using System.Collections.Generic;
using System.Web;
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

    public int QuestionsCount;
    public int SetCount;

    public KnowledgeSummary KnowledgeSummary = new KnowledgeSummary();
    public GetStreaksDaysResult StreakDays = new GetStreaksDaysResult();

    public IList<Date> Dates = new List<Date>();
    public IList<AnswerHistory> AnswerRecent = new List<AnswerHistory>();

    public User User = new User();
    public int ReputationRank;
    public int ReputationTotal;

    public UIMessage Message;

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

        KnowledgeSummary = R<KnowledgeSummaryLoader>().Run(UserId);

        Dates = GetSampleDates.Run();
        AnswerRecent = R<GetLastAnswers>().Run(UserId, 4);
        StreakDays = R<GetStreaksDays>().Run(User);
    }
}