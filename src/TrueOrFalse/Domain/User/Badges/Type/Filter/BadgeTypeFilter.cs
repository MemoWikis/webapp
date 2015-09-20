using System;

public class BadgeTypeFilter
{
    public static Func<BadgeTypeFilterParams, bool> Time(int startHour, int endHour)
    {
        return param =>
        {
            return false;
        };
    }
}