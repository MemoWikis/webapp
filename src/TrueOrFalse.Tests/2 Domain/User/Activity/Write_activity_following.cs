using NUnit.Framework;

public class Write_activity_following : BaseTest
{
    [Test]
    public void Should_write_activity_on_follow_user()
    {
        //User1 follows User2
        //User2 follows nobody (but later follows User 3)
        //User3 follows User2 and User4
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
        var userRepo = R<UserRepo>();
        //user2.Followers.Add(user1);
        //user2.Followers.Add(user3);
        //user4.Followers.Add(user3);
        userRepo.AddFollower(user1, user2);
        userRepo.AddFollower(user3, user2);
        userRepo.AddFollower(user3, user4);

        R<UserRepo>().Update(user2);
        R<UserRepo>().Update(user4);

        //User2 now follows User3
        //was user3.Followers.Add(user2);
        userRepo.AddFollower(user2, user3);
        //User4 now follows User1
        userRepo.AddFollower(user4, user1);

        //User1 should see activity: "User2 now follows User3"
        var activitiesUser1 = R<UserActivityRepo>().GetByUser(user1);
        Assert.That(activitiesUser1.Count, Is.EqualTo(1));
        Assert.That(activitiesUser1[0].Type, Is.EqualTo(UserActivityType.FollowedUser));
        Assert.That(activitiesUser1[0].UserCauser, Is.EqualTo(user2));
        Assert.That(activitiesUser1[0].UserIsFollowed, Is.EqualTo(user3));

        //User3 should see activity: "User2 now follows User3" (="User2 now follows you") and "User4 now follows User1"
        var activitiesUser3 = R<UserActivityRepo>().GetByUser(user3);
        Assert.That(activitiesUser3.Count, Is.EqualTo(2));
        foreach (var activityUser3 in activitiesUser3)
        {
            Assert.That(activityUser3.Type, Is.EqualTo(UserActivityType.FollowedUser));
            Assert.That(activityUser3.UserCauser, Is.EqualTo(user2).Or.EqualTo(user4));
        }
    }
}