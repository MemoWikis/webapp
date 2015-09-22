using NUnit.Framework;
using TrueOrFalse.Tests;

public class Badge_awarder : BaseTest
{
    [Test]
    public void Should_award_badges()
    {
        var user = ContextUser.GetUser();

        ContextQuestion.New()
            .PersistImmediately()
            .SetLearner(user)
            .AddQuestion()
                .AddAnswers(countCorrect: 1, countWrong: 0)
                .AddToWishknowledge(user);

        BadgeAwarder.Run(BadgeCheckOn.Answer, user);

        var allBadges = R<BadgeRepo>().GetAll();
        Assert.That(allBadges.Count, Is.EqualTo(1));
    }
}
