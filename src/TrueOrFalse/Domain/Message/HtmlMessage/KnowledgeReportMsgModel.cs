using System;
using System.Linq;
using System.Text;
using Seedworks.Web.State;
using TrueOrFalse.Frontend.Web.Code;

public class KnowledgeReportMsgModel
{
    public DateTime ShowStatsForPeriodSince;
    public string ShowStatsForPeriodSinceString;

    public string EmailTitle = "Dein wöchentlicher Wissensbericht";

    public string QuestionCountWish;
    public string SetCountWish;

    public string KnowledgeSolid;
    public string KnowledgeSolidPercentage;
    public string KnowledgeNeedsConsolidation;
    public string KnowledgeNeedsConsolidationPercentage;
    public string KnowledgeNeedsLearning;
    public string KnowledgeNeedsLearningPercentage;
    public string KnowledgeNotLearned;
    public string KnowledgeNotLearnedPercentage;

    public string KnowledgeLastLearnedDate;
    public string KnowledgeLastLearnedDateAsDistance;

    public string DaysLearnedSinceCount;
    public string PossibleLearningDaysSince;
    public string AnsweredQuestionsSinceCount;
    public string AnsweredQuestionsCorrectSinceCount;
    public string AnsweredQuestionsCorrectSincePercentage;
    public string StreakSince;
    public string TopStreak;
    public string TopStreakPeriod;

    public string NewQuestions;
    public string NewSets;
    public string TotalAvailableQuestions;
    public string TotalAvailableSets;

    public string UpcomingDates;
    public string DatesInNetwork;
    public string UpcomingTrainingDatesCount;
    public string UpcomingTrainingDatesTrainingTime;

    public string UnreadMessagesCount;
    public string FollowerIAm;
    public string FollowedIAm;

    public string LinkToWishQuestions;
    public string LinkToWishSets;
    public string LinkToLearningSession;
    public string LinkToDates;
    public string LinkToTechInfo;

    public string UtmSourceFullString;
    public string UtmCampaignFullString = "";

    public KnowledgeReportMsgModel(User user, string utmSource)
    {
        UtmSourceFullString = "&utm_source=" + utmSource;

        switch (user.KnowledgeReportInterval)
        {
            case UserSettingNotificationInterval.NotSet:
                goto case UserSettingNotificationInterval.Weekly; //defines the standard behaviour if setting is not set; needs to be the same as in UserSettings.aspx
            case UserSettingNotificationInterval.Daily:
                goto case UserSettingNotificationInterval.Weekly; // even if interval is daily, show stats for last seven days
            case UserSettingNotificationInterval.Weekly:
                ShowStatsForPeriodSince = DateTime.Now.AddDays(-7);
                ShowStatsForPeriodSinceString = "der letzten Woche";
                break;
            case UserSettingNotificationInterval.Monthly:
                ShowStatsForPeriodSince = DateTime.Now.AddMonths(-1);
                ShowStatsForPeriodSinceString = "dem letzten Monat";
                break;
            case UserSettingNotificationInterval.Quarterly:
                ShowStatsForPeriodSince = DateTime.Now.AddMonths(-3);
                ShowStatsForPeriodSinceString = "den letzten drei Monaten";
                break;
            default:
                goto case UserSettingNotificationInterval.Weekly;
        }


        QuestionCountWish = user.WishCountQuestions + " Frage" + StringUtils.PluralSuffix(user.WishCountQuestions, "n");
        SetCountWish = user.WishCountSets + " Lernset" + StringUtils.PluralSuffix(user.WishCountSets, "s");

        var knowledgeSummary = KnowledgeSummaryLoader.Run(user.Id);
        KnowledgeSolid = knowledgeSummary.Solid.ToString();
        KnowledgeSolidPercentage = knowledgeSummary.SolidPercentage.ToString();
        KnowledgeNeedsConsolidation = knowledgeSummary.NeedsConsolidation.ToString();
        KnowledgeNeedsConsolidationPercentage = knowledgeSummary.NeedsConsolidationPercentage.ToString();
        KnowledgeNeedsLearning = knowledgeSummary.NeedsLearning.ToString();
        KnowledgeNeedsLearningPercentage = knowledgeSummary.NeedsLearningPercentage.ToString();
        KnowledgeNotLearned = knowledgeSummary.NotLearned.ToString();
        KnowledgeNotLearnedPercentage = knowledgeSummary.NotLearnedPercentage.ToString();

        var knowledgeLastLearnedDate = Sl.R<LearningSessionRepo>().GetDateOfLastWishSession(user);
        if (knowledgeLastLearnedDate == null)
        {
            KnowledgeLastLearnedDate = "noch nie";
            KnowledgeLastLearnedDateAsDistance = "";
        }
        else
        {
            KnowledgeLastLearnedDate = knowledgeLastLearnedDate.Value.ToString("'zuletzt am' dd.MM.yyyy 'um' HH:mm");
            var remainingLabel = new TimeSpanLabel(DateTime.Now - (DateTime) knowledgeLastLearnedDate, useDativ: true);
            KnowledgeLastLearnedDateAsDistance = "<br />(Vor " + remainingLabel.Full + ")";
        }


        /* User's stats about recent learning behaviour */

        PossibleLearningDaysSince =
            (DateTime.Now -
             ((ShowStatsForPeriodSince < user.DateCreated) ? user.DateCreated : ShowStatsForPeriodSince)).Days
                .ToString();
        var answerStats = Sl.R<GetAnswerStatsInPeriod>().Run(user.Id, ShowStatsForPeriodSince.Date, DateTime.Now, groupByDate: true, excludeAnswerViews: true);
        DaysLearnedSinceCount = answerStats.Count.ToString();
        var answeredQuestionsSinceCount = answerStats.Sum(d => d.TotalAnswers);
        AnsweredQuestionsSinceCount = answeredQuestionsSinceCount.ToString() + " Frage" + StringUtils.PluralSuffix(answerStats.Sum(d => d.TotalAnswers),"n");
        if (answeredQuestionsSinceCount > 0)
        {
            var answeredQuestionsCorrectSinceCount = answerStats.Sum(d => d.TotalTrueAnswers);
            AnsweredQuestionsCorrectSinceCount = ",<br/> davon " + answeredQuestionsCorrectSinceCount.ToString() + " richtig (=";
            AnsweredQuestionsCorrectSinceCount += ((int)Math.Round(answeredQuestionsCorrectSinceCount / (float)answeredQuestionsSinceCount * 100)).ToString() + " %)";
        }

        var streak = Sl.R<GetStreaksDays>().Run(user, seperateStreakInRecentPeriodSince: ShowStatsForPeriodSince.Date);
        StreakSince = streak.RecentPeriodSLongestLength.ToString() + " Lerntag" + StringUtils.PluralSuffix(streak.RecentPeriodSLongestLength, "en");
        TopStreak = streak.LongestLength.ToString() + " Tag" + StringUtils.PluralSuffix(streak.LongestLength, "e");
        if (streak.RecentPeriodSLongestLength > 1)
            StreakSince += " (" + streak.RecentPeriodSLongestStart.ToString("dd.MM") + " - " + streak.RecentPeriodSLongestEnd.ToString("dd.MM.yyyy") + ")";
        else if (streak.RecentPeriodSLongestLength == 1)
            StreakSince += " (" + streak.RecentPeriodSLongestStart.ToString("dd.MM.yyyy") + ")";

        if (streak.LongestLength > 1)
            TopStreak += " (" + streak.LongestStart.ToString("dd.MM") + " - " + streak.LongestEnd.ToString("dd.MM.yyyy") + ")";
        else if (streak.LongestLength == 1)
            TopStreak += " (" + streak.LongestStart.ToString("dd.MM.yyyy") + ")";



        /* Stats on new content */

        NewQuestions = Sl.R<QuestionRepo>().HowManyNewPublicQuestionsCreatedSince(ShowStatsForPeriodSince).ToString();
        NewSets = Sl.R<SetRepo>().HowManyNewSetsCreatedSince(ShowStatsForPeriodSince).ToString();
        TotalAvailableQuestions = Sl.R<QuestionRepo>().TotalPublicQuestionCount().ToString();
        TotalAvailableSets = Sl.R<SetRepo>().TotalSetCount().ToString();


        /* User's dates and training sessions */

        var upcomingDates = Sl.R<DateRepo>().GetBy(user.Id, onlyUpcoming: true);
        var sbUpcomingDates = new StringBuilder();
        if (!upcomingDates.Any())
        {
            sbUpcomingDates.AppendLine("Du hast gerade keinen Termin. ");
            sbUpcomingDates.AppendLine("<a href = \"https://memucho.de/Termin/Erstellen?utm_medium=email" + UtmSourceFullString +
                            UtmCampaignFullString + "&utm_term=createDate\">Termin erstellen</a>.");
        }
        else
        {
            sbUpcomingDates.AppendLine("Du lernst gerade für " + upcomingDates.Count + " Termin" + StringUtils.PluralSuffix(upcomingDates.Count, "e") + ":");
            foreach (var date in upcomingDates)
            {
                sbUpcomingDates.AppendLine("<br/><strong>-&nbsp;" + date.GetTitle() + "</strong> (in " + new TimeSpanLabel(date.Remaining()).Full + ")");
            }
        }
        UpcomingDates = sbUpcomingDates.ToString();

        var datesInNetworkCount = Sl.R<GetDatesInNetwork>().Run(user.Id).Count;
        DatesInNetwork = datesInNetworkCount.ToString() + " Termin" + StringUtils.PluralSuffix(datesInNetworkCount, "e");
        
        var upcomingTrainingDates = Sl.R<TrainingDateRepo>().GetUpcomingTrainingDates();
        UpcomingTrainingDatesCount = upcomingTrainingDates.Count.ToString();
        if (upcomingTrainingDates.Count > 0)
        {
            var trainingTimeTimeSpan = new TimeSpan();
            trainingTimeTimeSpan = upcomingTrainingDates.Aggregate(trainingTimeTimeSpan, (current, upcomingTrainingDate) => current.Add(upcomingTrainingDate.TimeEstimated())); //adds up al TimeEstimated
            UpcomingTrainingDatesTrainingTime = " mit einer geschätzten Lernzeit von insgesamt " + trainingTimeTimeSpan.ToString("hh'h:'mm'min'");

        }


        /* User's additional status & infos */

        UnreadMessagesCount = Sl.R<GetUnreadMessageCount>().Run(user.Id).ToString();
        var followerIAmCount = Sl.R<UserRepo>().GetById(user.Id).Followers.Count; //needs to be reloaded for avoiding lazy-load problems
        var followedIAmCount = Sl.R<UserRepo>().GetById(user.Id).Followers.Count;
        FollowerIAm = followerIAmCount.ToString() + " Nutzer" + StringUtils.PluralSuffix(followerIAmCount, "n");
        FollowedIAm = followedIAmCount.ToString() + " Nutzer folg" + StringUtils.PluralSuffix(followedIAmCount, "en", "t");


        /* Create Links */

        LinkToWishQuestions = "https://memucho.de/Fragen/Wunschwissen";
        LinkToWishSets = "https://memucho.de/Fragesaetze/Wunschwissen";
        LinkToLearningSession = "https://memucho.de/Lernen/Wunschwissen";
        LinkToDates = "https://memucho.de/Termine";
        LinkToTechInfo = "https://memucho.de/AlgoInsight/Forecast";
    }
}
