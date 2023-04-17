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
            var oldUserLevel = SessionUser.User.ActivityLevel;
            activityPoints.UserId = SessionUser.UserId;
            Sl.ActivityPointsRepo.Create(activityPoints);
            Sl.UserRepo.UpdateActivityPointsData();

            var activityLevel = SessionUser.User.ActivityLevel;
            var activityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(activityLevel);
            var activityPointsTillNextLevel = activityPointsAtNextLevel - SessionUser.User.ActivityPoints;
            var activityPointsPercentageOfNextLevel = SessionUser.User.ActivityPoints == 0 ? 0 : 100 * SessionUser.User.ActivityPoints / activityPointsAtNextLevel;

            return Json(new 
                {
                    points = SessionUser.User.ActivityPoints,
                    level = activityLevel,
                    levelUp = oldUserLevel < activityLevel,
                    activityPointsTillNextLevel = activityPointsTillNextLevel,
                    activityPointsPercentageOfNextLevel
                });
        } 
        else 
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(SessionUser.GetTotalActivityPoints());
            SessionUser.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(SessionUser.GetTotalActivityPoints());

            return Json(new
                {
                    points = SessionUser.GetTotalActivityPoints(),
                    level = newUserLevel,
                    levelUp = oldUserLevel < newUserLevel,
                    activityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(newUserLevel)
                });
        }
    }

}