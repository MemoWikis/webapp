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

        ActivityPoints = 3578;
        ActivityLevel = UserLevelCalculator.GetLevel(ActivityPoints);
        ActivityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(3);
        ActivityPointsTillNextLevel = ActivityPointsAtNextLevel - ActivityPoints;
        ActivityPointsPercentageOfNextLevel = ActivityPoints == 0 ? 0 : 100 * ActivityPoints / ActivityPointsAtNextLevel;

    }

}
