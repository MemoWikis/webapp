using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Domain.User.Activity.ActivityPoints;
using TrueOrFalse.Search;

public class ActivityPointsApiController : BaseController
{
    public JsonResult AddActivityPoints(int points, string activityTypeString)
    {
        var activityType = (PointAction)Enum.Parse(typeof(PointAction), activityTypeString);
        Sl.R<SessionUser>().AddPointActivity(points, activityType);

        int totalActivityPoints = 0;
        foreach (var activity in R<SessionUser>().ActivityPoints)
        {
            totalActivityPoints += activity.Points;
        }
        return new JsonResult {Data = new { totalPoints = totalActivityPoints }};
    }
}