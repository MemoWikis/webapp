using System.Linq;
using NUnit.Framework;

public class Follower_persistence : BaseTest
{
    [Test]
    public void Should_Persist()
    {
        var userWritingRepo = R<UserWritingRepo>();
        var context = ContextUser.New(userWritingRepo)
            .Add("User 1")
            .Add("User 2")
            .Add("User 3")
            .Add("User 4")
            .Persist();

        var user1 = context.All[0];
        var user2 = context.All[1];
        var user3 = context.All[2];
        var user4 = context.All[3];

        userWritingRepo.AddFollower(user1, user1);
        userWritingRepo.AddFollower(user1, user1);
        userWritingRepo.AddFollower(user1, user1);

        userWritingRepo.Update(user1);

        RecycleContainer();

       var userReadingRepo = R<UserReadingRepo>();
        var userFromDb1 = userReadingRepo.GetById(user1.Id);
        var userFromDb2 = userReadingRepo.GetById(user2.Id);

        Assert.That(userFromDb1.Followers.Count, Is.EqualTo(3));
        Assert.That(userFromDb2.Following.Count, Is.EqualTo(1));

        RecycleContainer();

        var followerCounts = R<FollowerCounts>().Init(context.All.Select(u => u.Id));

        Assert.That(followerCounts.ByUserId(user1.Id), Is.EqualTo(3));
        Assert.That(followerCounts.ByUserId(user2.Id), Is.EqualTo(0));

        var followerIAm = R<FollowerIAm>().Init(context.All.Select(u => u.Id), user3.Id);

        Assert.That(followerIAm.Of(user1.Id), Is.True);
        Assert.That(followerIAm.Of(user2.Id), Is.False);

        Assert.That(R<TotalFollowers>().Run(user1.Id), Is.EqualTo(3));
        Assert.That(R<TotalIFollow>().Run(user2.Id), Is.EqualTo(1));
    }
}
