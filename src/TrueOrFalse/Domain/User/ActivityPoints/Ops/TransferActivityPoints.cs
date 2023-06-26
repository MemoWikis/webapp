public class TransferActivityPoints
{
    public static void FromSessionToUser(SessionUser sessionUser)
    {
        if (sessionUser.ActivityPoints.Count > 0)
        {
            foreach (var activityPoints in sessionUser.ActivityPoints)
            {
                activityPoints.UserId = sessionUser.UserId;
                if(activityPoints.UserId <= 0) 
                    continue;
                Sl.ActivityPointsRepo.Create(activityPoints);
            }
            sessionUser.ActivityPoints.Clear();
        }
    }
}