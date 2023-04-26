using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using VueApp;


namespace TrueOrFalse.Tests;

public class TopicControllerLogicTests : BaseTest
{

    [Test(Description = "Test SaveTopic Date to Low")]
    public void SaveTopicTestDateToLow()
    {
       var categoryContext = ContextCategory.New();
        var contextUser = ContextUser.New();

        var user = contextUser.Add(new User
        {
            Id = 1,
            Name = "Daniel",
            SubscriptionDuration = DateTime.Now.AddHours(-1),
            IsInstallationAdmin = true
        }) 
            .Persist()
            .All
            .First();
        SessionUser.Login(user);

        categoryContext
            .Add(new Category
            {
                Name = "private1",
                Creator = user, 
                Visibility = CategoryVisibility.Owner
            })
            .Add(new Category
            {
                Name = "private2",
                Creator = user,
                Visibility = CategoryVisibility.Owner
            })
            .Add(new Category
            {
                Name = "private3",
                Creator = user,
                Visibility = CategoryVisibility.Owner
            })
            .Persist();

        FieldInfo field = typeof(PermissionCheck)
            .GetField("_privateTopicsQuantity",  BindingFlags.Static | BindingFlags.NonPublic);
        field.SetValue(null, 2);


        var logik = new TopicControllerLogic();
        var result = logik.SaveTopic(categoryContext.All.First().Id,
            "private4",
            true,
            "nix drin",
            true);

        var expectedValue =
            JsonConvert.SerializeObject("Möglicherweise sollten Sie einige private Themen öffentlich machen" +
                                        " und ein Abonnement in Betracht ziehen, um mehr Funktionen zu erhalten.");
        Assert.AreEqual(expectedValue, result);
    }

    [Test(Description = "Test SaveTopic Date Ok")]
    public void SaveTopicToManyPrivateCategories()
    {
        var categoryContext = ContextCategory.New();
        var contextUser = ContextUser.New();

        var user = contextUser.Add(new User
            {
                Id = 1,
                Name = "Daniel",
                SubscriptionDuration = DateTime.Now.AddHours(2),
                IsInstallationAdmin = true
            })
            .Persist()
            .All
            .First();
        SessionUser.Login(user);

        categoryContext
            .Add("private1", creator: user)
            .Add("private2", creator: user)
            .Add("private3", creator: user)
            .Persist();

        FieldInfo field = typeof(PermissionCheck).GetField("_privateTopicsQuantity", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);


        var logik = new TopicControllerLogic();
        var result = logik.SaveTopic(categoryContext.All.First().Id,
            "private4",
            true,
            "nix drin",
            true);

        var expectedValue =
            JsonConvert.SerializeObject(true);
        Assert.AreEqual(expectedValue, result);
    }

    [Test(Description = "Test SaveTopic Date to Low and Count Categories Ok")]
    public void SaveTopicToManyPrivateCategoriesOkAndDateToLow()
    {
        var categoryContext = ContextCategory.New();
        var contextUser = ContextUser.New();

        var user = contextUser.Add(new User
            {
                Id = 1,
                Name = "Daniel",
                SubscriptionDuration = DateTime.Now.AddHours(-2),
                IsInstallationAdmin = true
            })
            .Persist()
            .All
            .First();
        SessionUser.Login(user);

        categoryContext
            .Add("private1", creator: user)
            .Persist();

        FieldInfo field = typeof(PermissionCheck).GetField("_privateTopicsQuantity", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);


        var logik = new TopicControllerLogic();
        var result = logik.SaveTopic(categoryContext.All.First().Id,
            "private4",
            true,
            "nix drin",
            true);

        var expectedValue =
            JsonConvert.SerializeObject(true);
        Assert.AreEqual(expectedValue, result);
    }
}

