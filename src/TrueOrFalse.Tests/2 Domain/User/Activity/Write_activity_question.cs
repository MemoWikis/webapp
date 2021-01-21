using NUnit.Framework;
using TrueOrFalse.Tests;

public class Write_activity_question : BaseTest
{
    [Test]
    //[Ignore("https://www.notion.so/bitwerke/Probleme-9cd8c8303c6344cd9eac58c5424182cc")]
    public void Should_write_activity_on_question_save()
    {
        //User1 follows User2 and User4
        //User2 follows User3 (and creates two questions)
        //User3 follows User4
        //User4 follows nobody (but creates one question)
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

        user4.AddFollower(user3);
        user4.AddFollower(user1);
        user2.AddFollower(user1);

        R<UserRepo>().Update(user4);
        R<UserRepo>().Update(user3);

        //User4 creates one question
        System.Threading.Thread.Sleep(50); // to much commits without sleep
        ContextQuestion.New().AddQuestion(creator: user4).Persist();
        //User2 creates two questions
        ContextQuestion.New()
            .AddQuestion(creator: user2)
            .AddQuestion(creator: user2)
            .Persist();
        
        //User3 should see activity: User4 created Question
        var activitiesUser3 = R<UserActivityRepo>().GetByUser(user3);
        Assert.That(activitiesUser3.Count, Is.EqualTo(1));
        Assert.That(activitiesUser3[0].Type, Is.EqualTo(UserActivityType.CreatedQuestion));

        //User1 should see activity: User4 created one question, User2 created two questions
        var activitiesUser1 = R<UserActivityRepo>().GetByUser(user1);
        Assert.That(activitiesUser1.Count, Is.EqualTo(3));
        foreach (var activityUser1 in activitiesUser1)
        {
            Assert.That(activityUser1.Type, Is.EqualTo(UserActivityType.CreatedQuestion));
        }

        //User2 should not see any activity
        var activitiesUser2 = R<UserActivityRepo>().GetByUser(user2);
        Assert.That(activitiesUser2.Count, Is.EqualTo(0));
    }
}