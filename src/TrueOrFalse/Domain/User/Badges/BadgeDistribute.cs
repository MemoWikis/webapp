using System.Windows.Forms;

public class BadgeDistribute
{
    public static void Run(BadgeCheckOn badgeCheckOn, User user)
    {
        var badgeRepo = Sl.R<BadgeRepo>();
        var allBadgesTypesToCheck = BadgeTypes.All().ByCheckOn(badgeCheckOn);

        foreach (var badgeType in allBadgesTypesToCheck)
        {
            if (badgeType.Awarded == null)
                continue;

            var result = badgeType.Awarded(new BadgeAwardCheckParams(badgeType, user));

            if (result.Success)
                badgeRepo.Create(result.Badge);
        }
    }
}