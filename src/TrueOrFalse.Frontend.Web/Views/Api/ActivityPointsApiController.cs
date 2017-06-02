using System;
using System.Web.Mvc;
             
public class ActivityPointsApiController : BaseController
{
    public JsonResult Add(string activityTypeString, int points)
    {
        var activityType = (ActivityPointsType)Enum.Parse(typeof(ActivityPointsType), activityTypeString);
        var isLoggedIn = Sl.SessionUser.IsLoggedInUser(Sl.SessionUser.UserId);
        var activityReward = new ActivityReward
        {
            Points = points,
            ActionType = activityType,
            DateEarned = DateTime.Now,
        };

        if(!isLoggedIn)
            Sl.SessionUser.AddPointActivity(activityReward);

        if (isLoggedIn)
        {
            activityReward.User = Sl.SessionUser.User;
            Sl.ActivityRewardRepo.Create(activityReward);
        }

        int totalActivityPoints = 0;
        foreach (var activity in R<SessionUser>().ActivityPoints)
        {
            totalActivityPoints += activity.Points;
        }
        return new JsonResult { Data = new { totalPoints = totalActivityPoints } };
    }
}