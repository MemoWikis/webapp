public class TransferActivityPoints
{
    public static void FromSessionToUser()
    {
        if (SessionUserLegacy.ActivityPoints.Count > 0)
        {
            foreach (var activityPoints in SessionUserLegacy.ActivityPoints)
            {
                activityPoints.UserId = SessionUserLegacy.UserId;
                if(activityPoints.UserId <= 0) 
                    continue;
                Sl.ActivityPointsRepo.Create(activityPoints);
            }
            SessionUserLegacy.ActivityPoints.Clear();
        }
    }
}