using NUnit.Framework;

public class Badge_params
{
    [Test]
    public void Should_get_badge_level()
    {
        var _1000_words_badge = BadgeTypes.All().ByKey("1000Words");

        var badgeParams = new BadgeAwardCheckParams(_1000_words_badge, new User());

        Assert.That(badgeParams.GetBadgeLevel(00), Is.Null);
        Assert.That(badgeParams.GetBadgeLevel(01).Name, Is.EqualTo(BadgeLevel.GetBronze().Name) );
        Assert.That(badgeParams.GetBadgeLevel(49).Name, Is.EqualTo(BadgeLevel.GetBronze().Name));
        Assert.That(badgeParams.GetBadgeLevel(50).Name, Is.EqualTo(BadgeLevel.GetSilver().Name));
        Assert.That(badgeParams.GetBadgeLevel(499).Name, Is.EqualTo(BadgeLevel.GetSilver().Name));
        Assert.That(badgeParams.GetBadgeLevel(500).Name, Is.EqualTo(BadgeLevel.GetGold().Name));
        Assert.That(badgeParams.GetBadgeLevel(500000).Name, Is.EqualTo(BadgeLevel.GetGold().Name));
    }
}
