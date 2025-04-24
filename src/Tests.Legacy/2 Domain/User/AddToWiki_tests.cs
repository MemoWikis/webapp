using System.Linq;
using NUnit.Framework;

public class AddToWiki_tests : BaseTest
{
    [Test]
    public void Add_Category_To_Wiki_Should_Set_Correct_HistoryField_In_User()
    {
        var user = new User();
        user.RecentlyUsedRelationTargetTopics = "";
        var userReadingRepo= R<UserReadingRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        R<UserWritingRepo>().Create(user);

        Assert.That(userReadingRepo.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo(""));

        RecentlyUsedRelationTargets.Add(user.Id, 3, userWritingRepo);
        Assert.That(userReadingRepo.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("3"));

        RecentlyUsedRelationTargets.Add(user.Id, 6, userWritingRepo);
        RecentlyUsedRelationTargets.Add(user.Id, 12, userWritingRepo);
        Assert.That(userReadingRepo.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("3,6,12"));

        RecentlyUsedRelationTargets.Add(user.Id, 4, userWritingRepo);
        Assert.That(userReadingRepo.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("6,12,4"));
    }
}