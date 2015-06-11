﻿using System.Linq;
using NUnit.Framework;

public class Follower_persistence : BaseTest
{
    [Test]
    public void Should_Persist()
    {
        var context = ContextUser.New()
            .Add("User 1")
            .Add("User 2")
            .Add("User 3")
            .Add("User 4")
            .Persist();

        var user1 = context.All[0];
        var user2 = context.All[1];
        var user3 = context.All[2];
        var user4 = context.All[3];

        user1.Followers.Add(user2);
        user1.Followers.Add(user3);
        user1.Followers.Add(user4);

        R<UserRepo>().Update(user1);

        RecycleContainer();

        var userRepo = R<UserRepo>();
        var userFromDb1 = userRepo.GetById(user1.Id);
        var userFromDb2 = userRepo.GetById(user2.Id);

        Assert.That(userFromDb1.Followers.Count, Is.EqualTo(3));
        Assert.That(userFromDb2.Following.Count, Is.EqualTo(1));

        RecycleContainer();

        var followerCounts = R<FollowerCounts>().Load(context.All.Select(u => u.Id));

        Assert.That(followerCounts.ByUserId(user1.Id), Is.EqualTo(3));
        Assert.That(followerCounts.ByUserId(user2.Id), Is.EqualTo(0));
    }
}
