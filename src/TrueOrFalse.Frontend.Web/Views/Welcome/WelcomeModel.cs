using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class WelcomeModel : BaseModel
{

    public string Date;

    public User User = new User();

    public bool UserHasWishknowledge => WishCount > 0;
    public int WishCount = 0;
    public KnowledgeSummary KnowledgeSummary = new KnowledgeSummary();
    public GetStreaksDaysResult StreakDays = new GetStreaksDaysResult();
    public IList<GetAnswerStatsInPeriodResult> Last30Days = new List<GetAnswerStatsInPeriodResult>();
    public bool HasLearnedInLast30Days;

    public int ActivityPoints;
    public int ActivityLevel;
    public int ActivityPointsAtNextLevel;
    public int ActivityPointsTillNextLevel;
    public int ActivityPointsPercentageOfNextLevel;

    public IList<int> CategoriesSchool;
    public IList<int> CategoriesUniversity;
    public IList<int> CategoriesCertificate;
    public IList<int> CategoriesGeneralKnowledge;

    public int TotalCategoriesCount;
    public int TotalCategoriesCountRound10;


    public WelcomeModel()
    {
        if (IsLoggedIn)
        {
            User = R<UserRepo>().GetById(UserId);

            WishCount = _sessionUser.User.WishCountQuestions;
            KnowledgeSummary = KnowledgeSummaryLoader.Run(UserId);
            var getAnswerStatsInPeriod = Resolve<GetAnswerStatsInPeriod>();
            Last30Days = getAnswerStatsInPeriod.GetLast30Days(UserId);
            HasLearnedInLast30Days = Last30Days.Sum(d => d.TotalAnswers) > 0;
            StreakDays = R<GetStreaksDays>().Run(User);
            ActivityPoints = User.ActivityPoints;
            ActivityLevel = User.ActivityLevel;
            ActivityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(ActivityLevel);
            ActivityPointsTillNextLevel = ActivityPointsAtNextLevel - ActivityPoints;
            ActivityPointsPercentageOfNextLevel = ActivityPoints == 0 ? 0 : 100 * ActivityPoints / ActivityPointsAtNextLevel;

        }
        else FillWithSampleData();

        TotalCategoriesCount = R<CategoryRepository>().TotalCategoryCount();
        TotalCategoriesCountRound10 = (int)Math.Floor(TotalCategoriesCount / 10.0) * 10;

        CategoriesUniversity = new List<int> { 706, 6, 741, 715, 806 };
        CategoriesSchool = new List<int> { 12, 686, 422, 231, 683, 744, 681, 746, 747, 795, 796 }; // 745,
        CategoriesCertificate = new List<int> { 393, 395, 467, 468, 388 };
        CategoriesGeneralKnowledge = new List<int> { 189, 58, 14, 84, 363, 196, 794, 203, 830};
    }

    private void FillWithSampleData()
    {
        KnowledgeSummary = new KnowledgeSummary
        {
            NotLearned = 25,
            NeedsLearning = 44,
            NeedsConsolidation = 91,
            Solid = 128
        };
        Last30Days = new List<GetAnswerStatsInPeriodResult>();
        int totalDayAnswers;
        var random = new Random();
        for (int i = 0; i < 30; i++)
        {
            totalDayAnswers = random.Next(0, 65);
            Last30Days.Add(new GetAnswerStatsInPeriodResult
            {
                DateTime = DateTime.Now.AddDays(-i),
                TotalAnswers = totalDayAnswers,
                TotalTrueAnswers = (int)Math.Ceiling((double)(random.Next(40, 101) / (double)100) * totalDayAnswers)
            });
        }
        HasLearnedInLast30Days = Last30Days.Sum(d => d.TotalAnswers) > 0;
        StreakDays = new GetStreaksDaysResult
        {
            LongestStart = DateTime.Now.AddDays(-123),
            LongestEnd = DateTime.Now.AddDays(-80),
            LongestLength = 43,
            LastStart = DateTime.Now.AddDays(-12),
            LastEnd = DateTime.Now,
            LastLength = 12,
            TotalLearningDays = 214
        };

        ActivityPoints = 3210;
        ActivityLevel = UserLevelCalculator.GetLevel(ActivityPoints);
        ActivityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(ActivityLevel);
        ActivityPointsTillNextLevel = ActivityPointsAtNextLevel - ActivityPoints;
        ActivityPointsPercentageOfNextLevel = ActivityPoints == 0 ? 0 : 100 * ActivityPoints / ActivityPointsAtNextLevel;

    }

}
