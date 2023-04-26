using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using Newtonsoft.Json;
using NUnit.Framework;
using VueApp;


namespace TrueOrFalse.Tests;

public class EditControllerLogicTests : BaseTest
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

        var search = A.Fake<IGlobalSearch>(); 
        var logik = new EditControllerLogic(search, true);
        var result = logik.QuickCreate("private4", -1);
        var resultJson = JsonConvert.SerializeObject(result);

        var expectedValue = JsonConvert.SerializeObject(new
        {success= false, message= "Möglicherweise sollten Sie einige private Themen öffentlich machen" +
                                                         " und ein Abonnement in Betracht ziehen, um mehr Funktionen zu erhalten."
        });
        Assert.AreEqual(resultJson, expectedValue);
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

        FieldInfo field = typeof(PermissionCheck).GetField("_privateTopicsQuantity", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);


        var search = A.Fake<IGlobalSearch>();
        var logik = new EditControllerLogic(search, true);
        var result = JsonConvert.SerializeObject(logik.QuickCreate("private4", categoryContext.All.First().Id));

        var expectedValue =
            JsonConvert.SerializeObject(new{success = true, url="", id=4});
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
            .Add(new Category
            {
                Name = "private1",
                Creator = user,
                Visibility = CategoryVisibility.Owner
            })
            .Persist();

        FieldInfo field = typeof(PermissionCheck).GetField("_privateTopicsQuantity", BindingFlags.NonPublic | BindingFlags.Static);
        field.SetValue(null, 2);


        var search = A.Fake<IGlobalSearch>();
        var logik = new EditControllerLogic(search, true);
        var result = JsonConvert.SerializeObject(logik.QuickCreate("private4", categoryContext.All.First().Id));

        var expectedValue =
            JsonConvert.SerializeObject(new { success = true, url = "", id = 2 });
        Assert.AreEqual(expectedValue, result);
    }
}

