using System;
using System.Web.Mvc;
             
public class ActivityPointsApiController : BaseController
{
    public JsonResult Add(string activityTypeString, int points)
    {
        var activityType = (PointAction)Enum.Parse(typeof(PointAction), activityTypeString);
        Sl.R<SessionUser>().AddPointActivity(points, activityType);

        int totalActivityPoints = 0;
        foreach (var activity in R<SessionUser>().ActivityPoints)
        {
            totalActivityPoints += activity.Points;
        }
        return new JsonResult { Data = new { totalPoints = totalActivityPoints } };
    }
}