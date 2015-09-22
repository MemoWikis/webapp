using System.Collections.Generic;
using System.Linq;

public static class BadgeTypesExt
{
    public static BadgeType ByKey(this IEnumerable<BadgeType> badgeTypes, string badgeKey)
    {
        return badgeTypes.SingleOrDefault(x => x.Key == badgeKey);
    }

    public static IEnumerable<BadgeType> ByGroupKey(this IEnumerable<BadgeType> badgeTypes, string groupKey)
    {
        return badgeTypes.Where(x => x.Group.Key == groupKey); 
    }

    public static IEnumerable<BadgeType> ByCheckOn(this IEnumerable<BadgeType> badgeTypes, BadgeCheckOn checkOn)
    {
        return badgeTypes.Where(
            x => 
                x.BadgeCheckOn != null && 
                x.BadgeCheckOn.Any(y => y == checkOn)
        );
    }
}