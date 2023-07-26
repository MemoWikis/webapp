using System;
using NHibernate;
using NUnit.Framework;

public class Should_retrieve_streak : BaseTest
{
    [Test]
    public void With_typical_data()
    {
        var user = ContextRegisteredUser.New(R<UserReadingRepo>(), R<UserWritingRepo>()).Add().Persist().Users[0];
        user.DateCreated = DateTime.Now.AddYears(-2);

        var ctx = R<ContextHistory>();
        ctx.WriteHistory(user, 0);
        ctx.WriteHistory(user, -1);
        ctx.WriteHistory(user, -10);
        ctx.WriteHistory(user, -11);
        ctx.WriteHistory(user, -12);
        ctx.WriteHistory(user, -13);
        ctx.WriteHistory(user, -17);

        var streakResult = R<GetStreaksDays>().Run(user);
        Assert.That(streakResult.LongestLength, Is.EqualTo(4));
        Assert.That(streakResult.LastLength, Is.EqualTo(2));
    }

    [Test]
    public void With_empty_data()
    {
        var user = ContextRegisteredUser
            .New(R<UserReadingRepo>(), R<UserWritingRepo>())
            .Add()
            .Persist()
            .Users[0];

        user.DateCreated = DateTime.Now.AddYears(-2);

        var streakResult = R<GetStreaksDays>().Run(user);
        Assert.That(streakResult.LongestLength, Is.EqualTo(0));
        Assert.That(streakResult.LastLength, Is.EqualTo(0));
    }


}