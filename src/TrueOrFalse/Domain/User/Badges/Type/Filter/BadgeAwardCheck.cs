using System;

public class BadgeAwardCheck
{

    public static Func<BadgeAwardCheckParams, BadgeAwardCheckResult> AlwaysFalse()
    {
        return param => new BadgeAwardCheckResult {Success = false};
    }

    public static Func<BadgeAwardCheckParams, BadgeAwardCheckResult> Get(Func<BadgeAwardCheckParams, BadgeLevel> fn)
    {
        return param =>
        {
            var badgeRepo = Sl.R<BadgeRepo>();

            if (badgeRepo.Exists(param.BadgeType.Key, param.CurrentUser.Id))
                return new BadgeAwardCheckResult { Success = false };

            var badgeLevel = fn(param);
            if (badgeLevel == null)
                return new BadgeAwardCheckResult { Success = false };

            return new BadgeAwardCheckResult
            {
                Success = true,
                Badge = new Badge
                {
                    BadgeTypeKey = param.BadgeType.Key,
                    Level = badgeLevel.Name,
                    User = param.CurrentUser
                }
            };
        };
    }
}