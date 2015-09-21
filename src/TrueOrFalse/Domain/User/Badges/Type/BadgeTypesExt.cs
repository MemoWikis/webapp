using System.Collections.Generic;
using System.Linq;

public static class BadgeTypesExt
{
    public static IEnumerable<BadgeType> ByGroupKey(this IEnumerable<BadgeType> badgeTypes, string groupKey)
    {
        return badgeTypes.Where(x => x.Group.Key == groupKey); 
    }
}