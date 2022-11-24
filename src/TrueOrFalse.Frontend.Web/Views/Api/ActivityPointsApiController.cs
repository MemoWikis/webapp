using System;
using System.Web.Mvc;

public class ActivityPointsApiController : BaseController
{
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
            activityPoints.User = SessionUser.User;
            Sl.ActivityPointsRepo.Create(activityPoints);
            Sl.UserRepo.UpdateActivityPointsData();
            //SessionUser.UpdateUser();

            var levelPopupAsHtml = "";
            if (oldUserLevel < SessionUser.User.ActivityLevel)
            {
                levelPopupAsHtml = ViewRenderer.RenderPartialView(
                    "~/Views/Api/ActivityPoints/LevelPopup.aspx",
                    new LevelPopupModel(SessionUser.User.ActivityLevel, SessionUser.User.ActivityPoints, true),
                    ControllerContext
                );
            }

            var activityLevel = SessionUser.User.ActivityLevel;
            var activityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(activityLevel);
            var activityPointsTillNextLevel = activityPointsAtNextLevel - SessionUser.User.ActivityPoints;
            var activityPointsPercentageOfNextLevel = SessionUser.User.ActivityPoints == 0 ? 0 : 100 * SessionUser.User.ActivityPoints / activityPointsAtNextLevel;

            return new JsonResult
            {
                Data = new
                {
                    totalPoints = SessionUser.User.ActivityPoints,
                    userLevel = activityLevel,
                    levelPopup = levelPopupAsHtml,
                    activityPointsTillNextLevel,
                    activityPointsPercentageOfNextLevel
                }
            };
        } 
        else 
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(SessionUser.GetTotalActivityPoints());
            SessionUser.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(SessionUser.GetTotalActivityPoints());

            var levelPopupAsHtml = "";
            if (oldUserLevel < newUserLevel)
                levelPopupAsHtml = ViewRenderer.RenderPartialView(
                    "~/Views/Api/ActivityPoints/LevelPopup.aspx", 
                    new LevelPopupModel(newUserLevel, SessionUser.GetTotalActivityPoints(), false), 
                    ControllerContext);

            return new JsonResult
            {
                Data = new
                {
                    totalPoints = SessionUser.GetTotalActivityPoints(), 
                    levelPopup = levelPopupAsHtml
                }
            };
        }
    }

}