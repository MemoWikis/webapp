using System.Linq;
using NUnit.Framework;

public class Badge_persistence : BaseTest
{
    [Test]
    public void Should_save_badges()
    {
        var badge = new Badge
        {
            Points = 0,
            BadgeTypeKey = BadgeTypes.All().First().Key,
            User = ContextUser.GetUser(),
            TimesGiven = 1,
            Level = "Bronze"
        };

        R<BadgeRepo>().Create(badge);
    }
}