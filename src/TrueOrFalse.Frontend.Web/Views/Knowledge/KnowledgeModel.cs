using System.Collections.Generic;
using TrueOrFalse;

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

    public GetAnswerStatsInPeriodResult AnswersThisWeek = new GetAnswerStatsInPeriodResult();
    public GetAnswerStatsInPeriodResult AnswersThisMonth = new GetAnswerStatsInPeriodResult();
    public GetAnswerStatsInPeriodResult AnswersThisYear = new GetAnswerStatsInPeriodResult();
    public GetAnswerStatsInPeriodResult AnswersLastMonth = new GetAnswerStatsInPeriodResult();
    public GetAnswerStatsInPeriodResult AnswersLastWeek = new GetAnswerStatsInPeriodResult();
    public GetAnswerStatsInPeriodResult AnswersLastYear = new GetAnswerStatsInPeriodResult();
    public GetAnswerStatsInPeriodResult AnswersEver = new GetAnswerStatsInPeriodResult();

    public int QuestionsCount;
    public int SetCount;

    public KnowledgeSummary KnowledgeSummary = new KnowledgeSummary();

    public IList<Date> Dates = new List<Date>();
    public IList<AnswerHistory> AnswerRecent = new List<AnswerHistory>();

    public User User = new User();
    public int ReputationRank;
    public int ReputationTotal;

    public KnowledgeModel()
    {
        if (!IsLoggedIn)
            return;

        QuestionsCount = R<GetWishQuestionCountCached>().Run(UserId);
        SetCount = R<GetWishSetCount>().Run(UserId);
        User = R<UserRepository>().GetById(UserId);

        var reputation = Resolve<ReputationCalc>().Run(User);
        ReputationRank = User.ReputationPos;
        ReputationTotal = reputation.TotalRepuation;

        var msg = new RecalcProbabilitiesMsg {UserId = UserId};
        Bus.Get().Publish(msg);

        var getAnswerStatsInPeriod = Resolve<GetAnswerStatsInPeriod>();
        AnswersThisWeek = getAnswerStatsInPeriod.RunForThisWeek(UserId);
        AnswersThisMonth = getAnswerStatsInPeriod.RunForThisMonth(UserId);
        AnswersThisYear = getAnswerStatsInPeriod.RunForThisYear(UserId);
        AnswersLastWeek = getAnswerStatsInPeriod.RunForLastWeek(UserId);
        AnswersLastMonth = getAnswerStatsInPeriod.RunForLastMonth(UserId);
        AnswersLastYear = getAnswerStatsInPeriod.RunForLastYear(UserId);
        AnswersEver = getAnswerStatsInPeriod.Run(UserId);

        KnowledgeSummary = R<KnowledgeSummaryLoader>().Run(UserId);

        Dates = GetSampleDates.Run();
        AnswerRecent = R<GetLastAnswers>().Run(UserId, 4);
    }
}