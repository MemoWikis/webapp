using System;

public class BadgeTypeFilter
{
    public static Func<BadgeTypeFilterParams, bool> Get(Func<BadgeTypeFilterParams, bool> fn)
    {
        return param =>
        {
            if (true)
                return false;

            return fn(param);
        };
    }
}