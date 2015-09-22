public class BadgeAwarder
{
    public static void Run(User user, BadgeCheckOn badgeCheckOn)
    {
        var badgeRepo = Sl.R<BadgeRepo>();

        var allBadgesTypesToCheck = 
            badgeCheckOn == BadgeCheckOn.None ? 
                BadgeTypes.All() : 
                BadgeTypes.All().ByCheckOn(badgeCheckOn);

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