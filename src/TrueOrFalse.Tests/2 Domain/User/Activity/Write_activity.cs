using NUnit.Framework;
using TrueOrFalse.Tests;

public class Write_activity : BaseTest
{
    [Test]
    public void Should_write_activity_on_question_save()
    {
        //User1 follows User2
        //User2 follows nobody (but creates one question)
        //User3 follows User1 (and creates two questions)
        //User4 follows User2 and User3
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

        user2.Followers.Add(user1);
        user2.Followers.Add(user4);
        user3.Followers.Add(user4);

        R<UserRepo>().Update(user2);
        R<UserRepo>().Update(user1);

        //User2 creates one question
        ContextQuestion.New().AddQuestion(creator: user2).Persist();
        //User3 creates two questions
        ContextQuestion.New()
            .AddQuestion(creator: user3)
            .AddQuestion(creator: user3)
            .Persist();
        
        //User1 should see activity: User2 created Question
        var activitiesUser1 = R<UserActivityRepo>().GetByUser(user1);
        Assert.That(activitiesUser1.Count, Is.EqualTo(1));
        Assert.That(activitiesUser1[0].Type, Is.EqualTo(UserActivityType.CreatedQuestion));

        //User4 should see activity: User2 created one question, User3 created two questions
        var activitiesUser4 = R<UserActivityRepo>().GetByUser(user4);
        Assert.That(activitiesUser4.Count, Is.EqualTo(3));
        foreach (var activityUser4 in activitiesUser4)
        {
            Assert.That(activityUser4.Type, Is.EqualTo(UserActivityType.CreatedQuestion));
        }

        //User3 should not see any activity
        var activitiesUser3 = R<UserActivityRepo>().GetByUser(user3);
        Assert.That(activitiesUser3.Count, Is.EqualTo(0));
       
    }
}