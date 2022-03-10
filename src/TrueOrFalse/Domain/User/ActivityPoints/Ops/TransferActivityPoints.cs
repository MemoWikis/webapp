public class TransferActivityPoints
{
    public static void FromSessionToUser()
    {
        if (SessionUser.ActivityPoints.Count > 0)
        {
            foreach (var activityPoints in SessionUser.ActivityPoints)
            {
                activityPoints.User = SessionUser.User;
                Sl.ActivityPointsRepo.Create(activityPoints);
            }
            SessionUser.ActivityPoints.Clear();
        }
    }
}