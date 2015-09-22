public class BadgeAwarder
{
    public static void Run(BadgeCheckOn badgeCheckOn, User user)
    {
        var badgeRepo = Sl.R<BadgeRepo>();
        var allBadgesTypesToCheck = BadgeTypes.All().ByCheckOn(badgeCheckOn);

        foreach (var badgeType in allBadgesTypesToCheck)
        {
            if (badgeType.AwardCheck == null)
                continue;

            var result = badgeType.AwardCheck(new BadgeAwardCheckParams(badgeType, user));

            if (result.Success)
                badgeRepo.Create(result.Badge);
        }
    }
}