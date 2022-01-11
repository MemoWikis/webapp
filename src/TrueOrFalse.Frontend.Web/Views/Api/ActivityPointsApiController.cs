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
            var oldUserLevel = _sessionUser.User.ActivityLevel;
            activityPoints.User = _sessionUser.User;
            Sl.ActivityPointsRepo.Create(activityPoints);
            Sl.UserRepo.UpdateActivityPointsData();
            _sessionUser.UpdateUser();

            var levelPopupAsHtml = "";
            if (oldUserLevel < _sessionUser.User.ActivityLevel)
            {
                levelPopupAsHtml = ViewRenderer.RenderPartialView(
                    "~/Views/Api/ActivityPoints/LevelPopup.aspx",
                    new LevelPopupModel(_sessionUser.User.ActivityLevel, _sessionUser.User.ActivityPoints, true),
                    ControllerContext
                );
            }

            return new JsonResult
            {
                Data = new
                {
                    totalPoints = _sessionUser.User.ActivityPoints,
                    userLevel = _sessionUser.User.ActivityLevel,
                    levelPopup = levelPopupAsHtml
                }
            };
        } 
        else 
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());
            _sessionUser.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());

            var levelPopupAsHtml = "";
            if (oldUserLevel < newUserLevel)
                levelPopupAsHtml = ViewRenderer.RenderPartialView(
                    "~/Views/Api/ActivityPoints/LevelPopup.aspx", 
                    new LevelPopupModel(newUserLevel, _sessionUser.GetTotalActivityPoints(), false), 
                    ControllerContext);

            return new JsonResult
            {
                Data = new
                {
                    totalPoints = _sessionUser.GetTotalActivityPoints(), 
                    levelPopup = levelPopupAsHtml
                }
            };
        }
    }

}