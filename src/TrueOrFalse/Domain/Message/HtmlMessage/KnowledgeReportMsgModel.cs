using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Util;
using Seedworks.Lib;
using Seedworks.Web.State;
using TrueOrFalse.Web;
using TrueOrFalse.Frontend.Web.Code;

public class KnowledgeReportMsgModel
{
    public DateTime LastSent;
    public string LastSentSinceDays;

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

    public KnowledgeReportMsgModel(User user, string utmSource = "knowledgeReportEmail")
    {
        LastSent = DateTime.Now.AddDays(-7); //todo: needs to be adapted if user can chose how often to receive update
        LastSentSinceDays = (DateTime.Now.Date - LastSent.Date).Days.ToString();
        UtmSourceFullString = string.IsNullOrEmpty(utmSource) ? "" : "&utm_source=" + utmSource;

        QuestionCountWish = user.WishCountQuestions + " Frage" + StringUtils.PluralSuffix(user.WishCountQuestions, "n");
        SetCountWish = user.WishCountSets + " Frage" + StringUtils.PluralSuffix(user.WishCountSets, "sätze", "satz");

        var knowledgeSummary = KnowledgeSummaryLoader.Run(user.Id);
        KnowledgeSolid = knowledgeSummary.Solid.ToString();
        KnowledgeSolidPercentage = knowledgeSummary.SolidPercentage.ToString();
        KnowledgeNeedsConsolidation = knowledgeSummary.NeedsConsolidation.ToString();
        KnowledgeNeedsConsolidationPercentage = knowledgeSummary.NeedsConsolidationPercentage.ToString();
        KnowledgeNeedsLearning = knowledgeSummary.NeedsLearning.ToString();
        KnowledgeNeedsLearningPercentage = knowledgeSummary.NeedsLearningPercentage.ToString();
        KnowledgeNotLearned = knowledgeSummary.NotLearned.ToString();
        KnowledgeNotLearnedPercentage = knowledgeSummary.NotLearnedPercentage.ToString();

        var knowledgeLastLearnedDate = Sl.R<LearningSessionRepo>().GetDateOfLastWishSession(user);// ?? DateTime.Now.AddDays(1);
        if (knowledgeLastLearnedDate == null)
        {
            KnowledgeLastLearnedDate = "noch nie";
            KnowledgeLastLearnedDateAsDistance = "";
        }
        else
        {
            KnowledgeLastLearnedDate = knowledgeLastLearnedDate?.ToString("'zuletzt am' dd.MM.yyyy 'um' HH:mm");
            var remainingLabel = new TimeSpanLabel(DateTime.Now - (knowledgeLastLearnedDate ?? DateTime.Now));
            KnowledgeLastLearnedDateAsDistance = "<br />(Vor " + remainingLabel.Full + ")";
        }


        /* User's stats about recent learning behaviour */

        var answerStats = Sl.R<GetAnswerStatsInPeriod>().Run(user.Id, LastSent.Date, DateTime.Now, groupByDate: true, excludeAnswerViews: true);
        DaysLearnedSinceCount = answerStats.Count.ToString();
        var answeredQuestionsSinceCount = answerStats.Sum(d => d.TotalAnswers);
        AnsweredQuestionsSinceCount = answeredQuestionsSinceCount.ToString() + " Frage" + StringUtils.PluralSuffix(answerStats.Sum(d => d.TotalAnswers),"n");
        if (answeredQuestionsSinceCount > 0)
        {
            var answeredQuestionsCorrectSinceCount = answerStats.Sum(d => d.TotalTrueAnswers);
            AnsweredQuestionsCorrectSinceCount = ",<br/> davon " + answeredQuestionsCorrectSinceCount.ToString() + " richtig (=";
            AnsweredQuestionsCorrectSinceCount += ((int)Math.Round(answeredQuestionsCorrectSinceCount / (float)answeredQuestionsSinceCount * 100)).ToString() + " %)";
        }

        var streak = Sl.R<GetStreaksDays>().Run(user, seperateStreakInRecentPeriodSince: LastSent.Date);
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

        NewQuestions = Sl.R<QuestionRepo>().HowManyNewPublicQuestionsCreatedSince(LastSent).ToString();
        NewSets = Sl.R<SetRepo>().HowManyNewSetsCreatedSince(LastSent).ToString();
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
            //sbUpcomingDates.AppendLine("<ul>");
            foreach (var date in upcomingDates)
            {
                sbUpcomingDates.AppendLine("<br/><strong>-&nbsp;" + date.GetTitle() + "</strong> (in " + new TimeSpanLabel(date.Remaining()).Full + ")");
            }
            //sbUpcomingDates.AppendLine("</ul>");
        }
        UpcomingDates = sbUpcomingDates.ToString();

        var datesInNetworkCount = Sl.R<GetDatesInNetwork>().Run(user.Id).Count;
        DatesInNetwork = datesInNetworkCount.ToString() + " Termin" + StringUtils.PluralSuffix(datesInNetworkCount, "e");
        
        var upcomingTrainingDates = Sl.R<TrainingDateRepo>().GetUpcomingTrainingDates(7);
        UpcomingTrainingDatesCount = upcomingTrainingDates.Count.ToString();
        var trainingTimeTimeSpan = new TimeSpan();
        trainingTimeTimeSpan = upcomingTrainingDates.Aggregate(trainingTimeTimeSpan, (current, upcomingTrainingDate) => current.Add(upcomingTrainingDate.TimeEstimated())); //adds up al TimeEstimated
        UpcomingTrainingDatesTrainingTime = trainingTimeTimeSpan.ToString("hh'h:'mm'min'");
        

        /* User's add. status & infos */

        UnreadMessagesCount = Sl.R<GetUnreadMessageCount>().Run(user.Id).ToString();
        var followerIAmCount = Sl.R<UserRepo>().GetById(user.Id).Followers.Count; //needs to be reloaded for avoiding lazy-load problems
        var followedIAmCount = Sl.R<UserRepo>().GetById(user.Id).Followers.Count;
        FollowerIAm = followerIAmCount.ToString() + " Nutzer" + StringUtils.PluralSuffix(followerIAmCount, "n"); ;
        FollowedIAm = followedIAmCount.ToString() + " Nutzer folg" + StringUtils.PluralSuffix(followedIAmCount, "en", "t");


        /* Create Links */

        if (ContextUtil.IsWebContext)
        {
            LinkToWishQuestions = Links.QuestionsWish();
            LinkToWishSets = Links.SetsWish();
            LinkToDates = Links.Dates();
            LinkToTechInfo = Links.AlgoInsightForecast();
        }
        else
        {
            LinkToWishQuestions = "https://memucho.de/Fragen/Wunschwissen";
            LinkToWishSets = "https://memucho.de/Fragesaetze/Wunschwissen";
            LinkToLearningSession = "https://memucho.de/Lernen/Wunschwissen";
            LinkToDates = "https://memucho.de/Termine";
            LinkToTechInfo = "https://memucho.de/AlgoInsight/Forecast";
        }
    }
}