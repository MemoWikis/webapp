using System;

public class UserLevelCalculator
{
    public static int GetLevel(int points)
    {

        if (points < 65)
            return 0;
        if (points >= 65 && points < 200)
            return 1;
        if (points >= 200 && points < 500)
            return 2;
        if (points >= 500 && points < 1000)
            return 3;
        if (points >= 1000 && points < 2000)
            return 4;
        if (points >= 2000 && points < 4000)
            return 5;
        if (points >= 4000 && points < CalculateUpperLevelBound(6))
            return 6;

        for (int level = 7; level <= 100; level++)
        {
            if(points >= CalculateLowerLevelBound(level) && points < CalculateUpperLevelBound(level))
                return level;
        }

        return 100;
    }

    private static int CalculateUpperLevelBound(int level) => (int)Math.Round(Math.Pow((level + 1) - 5, 1.9) * 2000);
    private static int CalculateLowerLevelBound(int level) => (int)Math.Round(Math.Pow(level - 5, 1.9) * 2000);

}