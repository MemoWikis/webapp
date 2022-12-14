public class TransferActivityPoints
{
    public static void FromSessionToUser()
    {
        if (SessionUser.ActivityPoints.Count > 0)
        {
            foreach (var activityPoints in SessionUser.ActivityPoints)
            {
                activityPoints.UserId = SessionUser.UserId;
                Sl.ActivityPointsRepo.Create(activityPoints);
            }
            SessionUser.ActivityPoints.Clear();
        }
    }
}