using NUnit.Framework;
using TrueOrFalse.Tests;

public class Write_activity_new_follower : BaseTest
{
    [Test]
    public void Should_write_activity_on_new_follower()
    {
        //NOT TESTED YET: Game created, because ContextGame.New().Add does not accept creator as parameter
        var context = ContextUser.New(R<UserRepo>())
            .Add("User 1")
            .Add("User 2")
            .Add("User 3")
            .Add("User 4")
            .Add("User 5")
            .Add("User 6")
            .Add("User 7")
            .Persist();

        var user1 = context.All[0];
        var user2 = context.All[1];
        var user3 = context.All[2];
        var user4 = context.All[3];
        var user5 = context.All[4];
        var user6 = context.All[5];
        var user7 = context.All[6];

        //SET-UP: USER2 THROUGH USER6 ALL DO SOMETHING
        //User2 creates two questions
        ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserRepo>(),
                R<CategoryRepository>())
            .AddQuestion(creator: user2)
            .AddQuestion(creator: user2)
            .Persist();
        //User3 creates one category
        ContextCategory.New().Add("Cat 1", creator: user3).Persist();
        var userRepo = R<UserRepo>(); 
        //User6 follows User7
        userRepo.AddFollower(user6, user7);

        RecycleContainer();

        //NEW FOLLOWER I: Now User1 follows User2
        userRepo.AddFollower(user1, user2);

        //User1 should see activity: User2 created two questions
        var activitiesUser1 = R<UserActivityRepo>().GetByUser(user1);
        Assert.That(activitiesUser1.Count, Is.EqualTo(2));
        Assert.That(activitiesUser1[0].Type, Is.EqualTo(UserActivityType.CreatedQuestion));
        Assert.That(activitiesUser1[0].UserCauser, Is.EqualTo(user2));
 
        //NEW FOLLOWER II: Now User1 also follows User3 through User6
        userRepo.AddFollower(user1, user3);
        userRepo.AddFollower(user1, user4);
        userRepo.AddFollower(user1, user5);
        userRepo.AddFollower(user1, user6);

        RecycleContainer();

        //User1 should see additional activity: User3 created one category; User4 one set; User5 one date; User6 follows User7
        activitiesUser1 = R<UserActivityRepo>().GetByUser(user1);
        Assert.That(activitiesUser1.Count, Is.EqualTo(4));
        foreach (var activityUser1 in activitiesUser1)
        {
            if (activityUser1.UserCauser == user3)
                Assert.That(activityUser1.Type, Is.EqualTo(UserActivityType.CreatedCategory));
            if (activityUser1.UserCauser == user4)
                Assert.That(activityUser1.Type, Is.EqualTo(UserActivityType.CreatedSet));
            if (activityUser1.UserCauser == user5)
                Assert.That(activityUser1.Type, Is.EqualTo(UserActivityType.CreatedDate));
            if (activityUser1.UserCauser == user6) {
                Assert.That(activityUser1.Type, Is.EqualTo(UserActivityType.FollowedUser));
                Assert.That(activityUser1.UserIsFollowed, Is.EqualTo(user7));
            }
        }

        //User3 should not see any activity
        var activitiesUser3 = R<UserActivityRepo>().GetByUser(user3);
        Assert.That(activitiesUser3.Count, Is.EqualTo(0));
       
    }
}