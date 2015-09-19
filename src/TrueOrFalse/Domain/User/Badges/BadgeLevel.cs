public class BadgeLevel
{
    public string Name;
    public int PointsNeeded;

    public static BadgeLevel GetGold(int pointsNeeded = 0)
    {
        return new BadgeLevel
        {
            Name = "Gold",
            PointsNeeded = pointsNeeded
        };
    }

    public static BadgeLevel GetSilver(int pointsNeeded = 0)
    {
        return new BadgeLevel
        {
            Name = "Silver",
            PointsNeeded = pointsNeeded
        };
    }

    public static BadgeLevel GetBronze(int pointsNeeded = 0)
    {
        return new BadgeLevel
        {
            Name = "Bronze",
            PointsNeeded = pointsNeeded
        };
    }
}