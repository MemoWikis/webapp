public class BadgeLevel
{
    public string Name;
    public int PointsNeeded;
    public int PointsAchieved;

    public static BadgeLevel GetGold(int pointsNeeded = -1, int pointsAchieved = -1)
    {
        return new BadgeLevel
        {
            Name = "Gold",
            PointsNeeded = pointsNeeded,
            PointsAchieved = pointsAchieved,
        };
    }

    public static BadgeLevel GetSilver(int pointsNeeded = -1, int pointsAchieved = -1)
    {
        return new BadgeLevel
        {
            Name = "Silver",
            PointsNeeded = pointsNeeded,
            PointsAchieved = pointsAchieved,
        };
    }

    public static BadgeLevel GetBronze(int pointsNeeded = -1, int pointsAchieved = -1)
    {
        return new BadgeLevel
        {
            Name = "Bronze",
            PointsNeeded = pointsNeeded,
            PointsAchieved = pointsAchieved,
        };
    }
}