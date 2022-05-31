using System;
using System.Linq;
using NUnit.Framework;

public class AddToWiki_tests : BaseTest
{

    [Test]
    public void Add_Category_To_Wiki_Should_Set_Correct_HistoryField_In_User()
    {
        var user = new User();
        user.AddToWikiHistory = "";
        var userRepository = Resolve<UserRepo>();
        userRepository.Create(user);

        Assert.That(userRepository.GetAll().FirstOrDefault().AddToWikiHistory, Is.EqualTo(""));

        user.AddNewIdToWikiHistory(3);
        Assert.That(userRepository.GetAll().FirstOrDefault().AddToWikiHistory, Is.EqualTo("3"));

        user.AddNewIdToWikiHistory(6);
        user.AddNewIdToWikiHistory(12);
        Assert.That(userRepository.GetAll().FirstOrDefault().AddToWikiHistory, Is.EqualTo("3,6,12"));

        user.AddNewIdToWikiHistory(4);
        Assert.That(userRepository.GetAll().FirstOrDefault().AddToWikiHistory, Is.EqualTo("6,12,4"));
    }
}