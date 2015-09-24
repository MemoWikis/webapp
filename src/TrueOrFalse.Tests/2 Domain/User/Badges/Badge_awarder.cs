using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Badge_awarder : BaseTest
{
    [Test]
    public void Should_award_badges()
    {
        var user = ContextUser.GetUser();

        var context = 
            ContextQuestion.New()
                .PersistImmediately()
                .SetLearner(user)
                .AddQuestion()
                    .AddAnswers(countCorrect: 1, countWrong: 0)
                    .AddToWishknowledge(user);

        for(var i = 0; i < 5; i++)
            BadgeAwarder.Run(user, BadgeCheckOn.None);

        var allBadges = R<BadgeRepo>().GetAll();
        Assert.That(allBadges.Count, Is.EqualTo(3));
        Assert.That(allBadges.Count(x => x.BadgeTypeKey == "NewbieBronze"), Is.EqualTo(1));
        Assert.That(allBadges.Count(x => x.BadgeTypeKey == "ThanksForHelp"), Is.EqualTo(1));
        Assert.That(allBadges.Count(x => x.BadgeTypeKey == "FasterThanShadow"), Is.EqualTo(1));
    }
}
