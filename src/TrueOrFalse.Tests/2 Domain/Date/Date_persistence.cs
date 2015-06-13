using System;
using NUnit.Framework;

public class Date_persistence : BaseTest
{
    [Test]
    public void Should_persist()
    {
        var allSets = ContextSet.New().AddSet("1").AddSet("1").Persist().All;
        var user = ContextUser.New().Add("user").Persist().All[0];

        var date = new Date();
        date.Details = "Details";
        date.Sets = allSets;
        date.User = user;
        date.Visibility = DateVisibility.Private;
        date.DateTime = DateTime.Now.AddDays(1);
        R<DateRepo>().Create(date);

        RecycleContainer();

        var dateFromDb = R<DateRepo>().GetById(date.Id);
        Assert.That(dateFromDb.Details, Is.EqualTo("Details"));
        Assert.That(dateFromDb.Visibility, Is.EqualTo(DateVisibility.Private));
        Assert.That(dateFromDb.Sets.Count, Is.EqualTo(2));
    }
}