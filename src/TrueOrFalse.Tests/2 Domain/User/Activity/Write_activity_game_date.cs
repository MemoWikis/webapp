using NUnit.Framework;
using TrueOrFalse.Tests;

public class Write_activity_game_date : BaseTest
{
    [Test]
    public void Should_write_activity_on_game_and_date_create()
    {
        //NOT YET IMPLEMENTED FOR CREATE-GAME: See reason in commented out section below
        
        //User1 follows User2 and User4
        //User2 follows User3 (and creates two dates)
        //User3 follows User4
        //User4 follows nobody (but creates one game)
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

        user4.Followers.Add(user3);
        user4.Followers.Add(user1);
        user2.Followers.Add(user1);

        R<UserRepo>().Update(user4);
        R<UserRepo>().Update(user3);


        //Test doesn't work for Create Game, because ContextGame.New().Add so far does not accept User/UserId as parameter to set Creator, 
        //which is necessary to test UserActivity

        ////User4 creates one game
        //ContextGame.New().Add().Persist(); //change Add to accept User/UserID as creator;

        //User2 creates two dates
        var contextSets = ContextSet.New().AddSet("QSet 1").AddSet("QSet 2").Persist();
        ContextDate.New()
            .Add(new[] { contextSets.All[0], contextSets.All[1] }, creator: user2)
            .Add(new[] { contextSets.All[1] }, creator: user2)
            .Persist();

        ////User3 should see activity: User4 created Game
        //var activitiesUser3 = R<UserActivityRepo>().GetByUser(user3);
        //Assert.That(activitiesUser3.Count, Is.EqualTo(1));
        //Assert.That(activitiesUser3[0].Type, Is.EqualTo(UserActivityType.CreatedGame));

        //User1 should see activity: User2 created two dates //and User 4 created Game
        var activitiesUser1 = R<UserActivityRepo>().GetByUser(user1);
        Assert.That(activitiesUser1.Count, Is.EqualTo(2)); //EqualTo(3) if Create-Game works
        foreach (var activityUser1 in activitiesUser1)  
        {
            //if (activityUser1.UserCauser == user4)
            //    Assert.That(activityUser1.Type, Is.EqualTo(UserActivityType.CreatedGame));
            if (activityUser1.UserCauser == user2)
                Assert.That(activityUser1.Type, Is.EqualTo(UserActivityType.CreatedDate));
        }

        //User2 should not see any activity
        var activitiesUser2 = R<UserActivityRepo>().GetByUser(user2);
        Assert.That(activitiesUser2.Count, Is.EqualTo(0));
       
    }
}