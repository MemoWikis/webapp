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

    public IList<WordpressBlogPost> MemuchoBlogPosts;

    public WelcomeModel()
    {
        if (IsLoggedIn)
        {
            User = R<UserRepo>().GetById(UserId);

            WishCount = _sessionUser.User.WishCountQuestions;
            KnowledgeSummary = KnowledgeSummaryLoader.Run(UserId);
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

       MemuchoBlogPosts = BlogMemuchoDeRepo.GetRecentPosts(3);
    }

    private void FillWithSampleData()
    {
        KnowledgeSummary = new KnowledgeSummary
        (
            notLearned : 25,
            needsLearning : 44,
            needsConsolidation : 91,
            solid : 128
        );

        ActivityPoints = 3210;
        ActivityLevel = UserLevelCalculator.GetLevel(ActivityPoints);
        ActivityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(ActivityLevel);
        ActivityPointsTillNextLevel = ActivityPointsAtNextLevel - ActivityPoints;
        ActivityPointsPercentageOfNextLevel = ActivityPoints == 0 ? 0 : 100 * ActivityPoints / ActivityPointsAtNextLevel;

    }

}
