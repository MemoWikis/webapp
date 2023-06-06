using System;
using System.Web.Mvc;

public class ActivityPointsStoreController : BaseController
{
    public ActivityPointsStoreController(SessionUser sessionUser): base(sessionUser)
    {
        
    }
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
            var oldUserLevel = _sessionUser.User.ActivityLevel;
            activityPoints.UserId = _sessionUser.UserId;
            Sl.ActivityPointsRepo.Create(activityPoints);
            Sl.UserRepo.UpdateActivityPointsData();

            var activityLevel = _sessionUser.User.ActivityLevel;
            var activityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(activityLevel);
            var activityPointsTillNextLevel = activityPointsAtNextLevel - _sessionUser.User.ActivityPoints;
            var activityPointsPercentageOfNextLevel = _sessionUser.User.ActivityPoints == 0 ? 0 : 100 * _sessionUser.User.ActivityPoints / activityPointsAtNextLevel;

            return Json(new 
                {
                    points = _sessionUser.User.ActivityPoints,
                    level = activityLevel,
                    levelUp = oldUserLevel < activityLevel,
                    activityPointsTillNextLevel = activityPointsTillNextLevel,
                    activityPointsPercentageOfNextLevel
                });
        } 
        else 
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());
            _sessionUser.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());

            return Json(new
                {
                    points = _sessionUser.GetTotalActivityPoints(),
                    level = newUserLevel,
                    levelUp = oldUserLevel < newUserLevel,
                    activityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(newUserLevel)
                });
        }
    }

}