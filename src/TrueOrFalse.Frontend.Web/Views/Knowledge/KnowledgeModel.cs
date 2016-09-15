using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Web;

public class KnowledgeModel : BaseModel
{
    public string UserName
    {
        get
        {
            return _sessionUser.User == null ? 
                "Unbekannte(r)" : 
                _sessionUser.User.Name;
        }
    }

    public IList<GetAnswerStatsInPeriodResult> Last30Days = new List<GetAnswerStatsInPeriodResult>();
    public bool HasLearnedInLast30Days;

    public int QuestionsCount;
    public int QuestionsCreatedCount;
    public int SetsCount;
    public int SetsCreatedCount;

    public KnowledgeSummary KnowledgeSummary = new KnowledgeSummary();
    public GetStreaksDaysResult StreakDays = new GetStreaksDaysResult();

    public IList<Date> Dates = new List<Date>();
    public IList<Date> DatesInNetwork = new List<Date>();
    public IList<Answer> AnswerRecent = new List<Answer>();
    public IList<TrainingDateModel> TrainingDates = new List<TrainingDateModel>();

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
        SetsCount = R<GetWishSetCount>().Run(UserId);
        User = R<UserRepo>().GetById(UserId);

        QuestionsCreatedCount = Resolve<UserSummary>().AmountCreatedQuestions(UserId);
        SetsCreatedCount = Resolve<UserSummary>().AmountCreatedSets(UserId);

        var reputation = Resolve<ReputationCalc>().Run(User);
        ReputationRank = User.ReputationPos;
        ReputationTotal = reputation.TotalReputation;

        var msg = new RecalcProbabilitiesMsg {UserId = UserId};
        //Bus.Get().Publish(msg);

        KnowledgeSummary = KnowledgeSummaryLoader.Run(UserId);

        var getAnswerStatsInPeriod = Resolve<GetAnswerStatsInPeriod>();
        Last30Days = getAnswerStatsInPeriod.GetLast30Days(UserId);
        HasLearnedInLast30Days = Last30Days.Sum(d => d.TotalAnswers) > 0;
        StreakDays = R<GetStreaksDays>().Run(User);

        //Dates = GetSampleDates.Run();
        Dates = R<DateRepo>().GetBy(UserId, true);
        DatesInNetwork = R<GetDatesInNetwork>().Run(UserId);

        AnswerRecent = R<AnswerRepo>().GetUniqueByUser(UserId, amount: 15);

        //GET NETWORK ACTIVITY
        NetworkActivities = R<UserActivityRepo>().GetByUser(User, 8);

        //GET TRAINING DATES
        var tdTrainingDates = R<TrainingDateRepo>().GetUpcomingTrainingDates(7);
        foreach (var tdTrainingDate in tdTrainingDates)
        {
            TrainingDates.Add(new TrainingDateModel(tdTrainingDate));
        }
        //TrainingDates = new List<TrainingDateModel>
        //{
        //    new TrainingDateModel
        //    {
        //        DateTime = DateTime.Now.AddHours(4),
        //        QuestionCount = 12,
        //        Date = new Date { Details = "Klassenarbeit DE"}
        //    },
        //    new TrainingDateModel
        //    {
        //        DateTime = DateTime.Now.AddHours(24),
        //        QuestionCount = 21,
        //        Date = new Date { Details = "Klassenarbeit DE"}
        //    },
        //    new TrainingDateModel
        //    {
        //        DateTime = DateTime.Now.AddHours(57),
        //        QuestionCount = 19,
        //        Date = new Date { Details = "Mündliche Prüfung am Fr."}
        //    },
        //    new TrainingDateModel
        //    {
        //        DateTime = DateTime.Now.AddHours(71),
        //        QuestionCount = 20,
        //    }
        //};

    }
}