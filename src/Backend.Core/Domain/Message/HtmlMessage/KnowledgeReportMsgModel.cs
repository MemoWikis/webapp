public class KnowledgeReportMsgModel
{
    private readonly GetAnswerStatsInPeriod _getAnswerStatsInPeriod;
    private readonly GetStreaksDays _getStreaksDays;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly GetUnreadMessageCount _getUnreadMessageCount;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly QuestionReadingRepo _questionReadingRepo;
    public DateTime ShowStatsForPeriodSince;
    public string ShowStatsForPeriodSinceString;

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

    public string DaysLearnedSinceCount;
    public string PossibleLearningDaysSince;
    public string AnsweredQuestionsSinceCount;
    public string AnsweredQuestionsCorrectSinceCount;
    public string StreakSince;
    public string TopStreak;

    public string NewQuestions;
    public string TotalAvailableQuestions;

    public string UnreadMessagesCount;
    public string FollowerIAm;
    public string FollowedIAm;

    public string LinkToWishQuestions;
    public string LinkToWishSets;
    public string LinkToLearningSession;
    public string LinkToDates;
    public string LinkToTechInfo;

    public string UtmSourceFullString;

    public KnowledgeReportMsgModel(
        User user,
        string utmSource,
        GetAnswerStatsInPeriod getAnswerStatsInPeriod,
        GetStreaksDays getStreaksDays,
        UserReadingRepo userReadingRepo,
        GetUnreadMessageCount getUnreadMessageCount,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        QuestionReadingRepo questionReadingRepo)
    {
        _getAnswerStatsInPeriod = getAnswerStatsInPeriod;
        _getStreaksDays = getStreaksDays;
        _userReadingRepo = userReadingRepo;
        _getUnreadMessageCount = getUnreadMessageCount;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _questionReadingRepo = questionReadingRepo;
        UtmSourceFullString = "&utm_source=" + utmSource;

        switch (user.KnowledgeReportInterval)
        {
            case UserSettingNotificationInterval.NotSet:
                goto case
                    UserSettingNotificationInterval
                        .Weekly; //defines the standard behaviour if setting is not set; needs to be the same as in UserSettings.aspx
            case UserSettingNotificationInterval.Daily:
                goto case
                    UserSettingNotificationInterval
                        .Weekly; // even if interval is daily, show stats for last seven days
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

        QuestionCountWish = user.WishCountQuestions + " Frage" +
                            StringUtils.PluralSuffix(user.WishCountQuestions, "n");
        SetCountWish = user.WishCountSets + " Lernset" +
                       StringUtils.PluralSuffix(user.WishCountSets, "s");

        var knowledgeSummary = _knowledgeSummaryLoader.Run(user.Id);
        // Combined values from InWishKnowledge and NotInWishKnowledge
        KnowledgeSolid = (knowledgeSummary.InWishKnowledge.Solid + knowledgeSummary.NotInWishKnowledge.Solid).ToString();
        KnowledgeSolidPercentage = knowledgeSummary.InWishKnowledge.SolidPercentage.ToString();
        KnowledgeNeedsConsolidation = (knowledgeSummary.InWishKnowledge.NeedsConsolidation + knowledgeSummary.NotInWishKnowledge.NeedsConsolidation).ToString();
        KnowledgeNeedsConsolidationPercentage = knowledgeSummary.InWishKnowledge.NeedsConsolidationPercentage.ToString();
        KnowledgeNeedsLearning = (knowledgeSummary.InWishKnowledge.NeedsLearning + knowledgeSummary.NotInWishKnowledge.NeedsLearning).ToString();
        KnowledgeNeedsLearningPercentage = knowledgeSummary.InWishKnowledge.NeedsLearningPercentage.ToString();
        KnowledgeNotLearned = (knowledgeSummary.InWishKnowledge.NotLearned + knowledgeSummary.NotInWishKnowledge.NotLearned).ToString();
        KnowledgeNotLearnedPercentage = knowledgeSummary.InWishKnowledge.NotLearnedPercentage.ToString();

        /* User's stats about recent learning behaviour */

        PossibleLearningDaysSince =
            (DateTime.Now -
             ((ShowStatsForPeriodSince < user.DateCreated)
                 ? user.DateCreated
                 : ShowStatsForPeriodSince)).Days
            .ToString();
        var answerStats = _getAnswerStatsInPeriod.Run(user.Id, ShowStatsForPeriodSince.Date,
            DateTime.Now, groupByDate: true, excludeAnswerViews: true);
        DaysLearnedSinceCount = answerStats.Count.ToString();
        var answeredQuestionsSinceCount = answerStats.Sum(d => d.TotalAnswers);
        AnsweredQuestionsSinceCount = answeredQuestionsSinceCount + " Frage" +
                                      StringUtils.PluralSuffix(answerStats.Sum(d => d.TotalAnswers),
                                          "n");
        if (answeredQuestionsSinceCount > 0)
        {
            var answeredQuestionsCorrectSinceCount = answerStats.Sum(d => d.TotalTrueAnswers);
            AnsweredQuestionsCorrectSinceCount =
                ",<br/> davon " + answeredQuestionsCorrectSinceCount + " richtig (=";
            AnsweredQuestionsCorrectSinceCount +=
                ((int)Math.Round(answeredQuestionsCorrectSinceCount /
                    (float)answeredQuestionsSinceCount * 100)) + " %)";
        }

        var streak = _getStreaksDays.Run(user,
            seperateStreakInRecentPeriodSince: ShowStatsForPeriodSince.Date);
        StreakSince = streak.RecentPeriodSLongestLength + " Lerntag" +
                      StringUtils.PluralSuffix(streak.RecentPeriodSLongestLength, "en");
        TopStreak = streak.LongestLength + " Tag" +
                    StringUtils.PluralSuffix(streak.LongestLength, "e");
        if (streak.RecentPeriodSLongestLength > 1)
            StreakSince += " (" + streak.RecentPeriodSLongestStart.ToString("dd.MM") + " - " +
                           streak.RecentPeriodSLongestEnd.ToString("dd.MM.yyyy") + ")";
        else if (streak.RecentPeriodSLongestLength == 1)
            StreakSince += " (" + streak.RecentPeriodSLongestStart.ToString("dd.MM.yyyy") + ")";

        if (streak.LongestLength > 1)
            TopStreak += " (" + streak.LongestStart.ToString("dd.MM") + " - " +
                         streak.LongestEnd.ToString("dd.MM.yyyy") + ")";
        else if (streak.LongestLength == 1)
            TopStreak += " (" + streak.LongestStart.ToString("dd.MM.yyyy") + ")";

        /* Stats on new content */

        NewQuestions = _questionReadingRepo
            .HowManyNewPublicQuestionsCreatedSince(ShowStatsForPeriodSince).ToString();
        TotalAvailableQuestions = _questionReadingRepo.TotalPublicQuestionCount().ToString();

        /* User's additional status & infos */

        UnreadMessagesCount = _getUnreadMessageCount.Run(user.Id).ToString();
        var followerIAmCount =
            _userReadingRepo.GetById(user.Id).Followers
                .Count; //needs to be reloaded for avoiding lazy-load problems
        var followedIAmCount = _userReadingRepo.GetById(user.Id).Followers.Count;
        FollowerIAm = followerIAmCount + " Nutzer" +
                      StringUtils.PluralSuffix(followerIAmCount, "n");
        FollowedIAm = followedIAmCount + " Nutzer folg" +
                      StringUtils.PluralSuffix(followedIAmCount, "en", "t");

        /* Create Links */

        LinkToWishQuestions = "https://memoWikis.de/Fragen/Wunschwissen";
        LinkToWishSets = "https://memoWikis.de/Fragesaetze/Wunschwissen";
        LinkToLearningSession = "https://memoWikis.de/Lernen/Wunschwissen";
        LinkToDates = "https://memoWikis.de/Termine";
        LinkToTechInfo = "https://memoWikis.de/AlgoInsight/Forecast";
    }
}