using System.Linq;
using NUnit.Framework;

public class AddToWiki_tests : BaseTest
{
    [Test]
    public void Add_Category_To_Wiki_Should_Set_Correct_HistoryField_In_User()
    {
        var user = new User();
        user.RecentlyUsedRelationTargetTopics = "";
        var userRepository = R<UserRepo>();
        userRepository.Create(user);

        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo(""));

        RecentlyUsedRelationTargets.Add(user.Id, 3, userRepository);
        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("3"));

        RecentlyUsedRelationTargets.Add(user.Id, 6, userRepository);
        RecentlyUsedRelationTargets.Add(user.Id, 12, userRepository);
        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("3,6,12"));

        RecentlyUsedRelationTargets.Add(user.Id, 4, userRepository);
        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("6,12,4"));
    }
}