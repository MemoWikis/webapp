using System;
using NUnit.Framework;

public class User_persistence : BaseTest
{
    [Test]
    public void Should_perist_user() 
    {
        var user = new User();
        user.Name = "Vorname Nachname";
        user.Birthday = new DateTime(1980, 08, 03);

        R<UserWritingRepo>().Create(user);

        Assert.That(R<UserReadingRepo>().GetAll().Count, Is.EqualTo(1));
    }
}