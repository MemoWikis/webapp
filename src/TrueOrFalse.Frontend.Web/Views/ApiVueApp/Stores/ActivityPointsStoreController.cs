using System;
using System.Web.Mvc;

public class ActivityPointsStoreController : BaseController
{
    [HttpPost]
    public JsonResult Add(string activityTypeString, int points)
    {
        var activityType = (ActivityPointsType)Enum.Parse(typeof(ActivityPointsType), activityTypeString);
        var activityPoints = new ActivityPoints
        {
            Amount = points,
            ActionType = activityType,
            DateEarned = DateTime.Now
        };

        if (IsLoggedIn)
        {
            var oldUserLevel = SessionUserLegacy.User.ActivityLevel;
            activityPoints.UserId = SessionUserLegacy.UserId;
            Sl.ActivityPointsRepo.Create(activityPoints);
            Sl.UserRepo.UpdateActivityPointsData();

            var activityLevel = SessionUserLegacy.User.ActivityLevel;
            var activityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(activityLevel);
            var activityPointsTillNextLevel = activityPointsAtNextLevel - SessionUserLegacy.User.ActivityPoints;
            var activityPointsPercentageOfNextLevel = SessionUserLegacy.User.ActivityPoints == 0 ? 0 : 100 * SessionUserLegacy.User.ActivityPoints / activityPointsAtNextLevel;

            return Json(new 
                {
                    points = SessionUserLegacy.User.ActivityPoints,
                    level = activityLevel,
                    levelUp = oldUserLevel < activityLevel,
                    activityPointsTillNextLevel = activityPointsTillNextLevel,
                    activityPointsPercentageOfNextLevel
                });
        } 
        else 
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(SessionUserLegacy.GetTotalActivityPoints());
            SessionUserLegacy.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(SessionUserLegacy.GetTotalActivityPoints());

            return Json(new
                {
                    points = SessionUserLegacy.GetTotalActivityPoints(),
                    level = newUserLevel,
                    levelUp = oldUserLevel < newUserLevel,
                    activityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(newUserLevel)
                });
        }
    }

}