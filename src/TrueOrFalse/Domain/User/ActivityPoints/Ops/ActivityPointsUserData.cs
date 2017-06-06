using System.Linq;

public class ActivityPointsUserData
{
    public static void Update()
    {
        var totalPointCount = 0;
        foreach (var activityPoints in Sl.ActivityPointsRepo.GetActivtyPointsByUser(Sl.CurrentUserId))
        {
            totalPointCount += activityPoints.Amount;
        }

        var userLevel = UserLevelCalculator.GetLevel(totalPointCount);

        var user = Sl.UserRepo.GetByIds(Sl.SessionUser.UserId).First();
        user.ActivityPoints = totalPointCount;
        user.ActivityLevel = userLevel;
        Sl.UserRepo.Update(user);
    }
}