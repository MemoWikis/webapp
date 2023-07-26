using System;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class UserActivity_persistence : BaseTest
{
    [Test]
    public void Should_persist()
    {
        var contextUser = ContextUser.New(R<UserReadingRepo>()).Add("Firstname Lastname").Persist();
        var contextCategory = ContextCategory.New().Add("Mathe2").Persist();

        var userActivity = new UserActivity
        {
            UserConcerned = contextUser.All[0],
            At = DateTime.Now,
            Category = contextCategory.All[0]
        };

        R<UserActivityRepo>().Create(userActivity);
        //todo: assert is missing, isn't it?
    }
}