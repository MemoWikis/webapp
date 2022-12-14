using System.Linq;
using NUnit.Framework;

public class AddToWiki_tests : BaseTest
{
    [Test]
    public void Add_Category_To_Wiki_Should_Set_Correct_HistoryField_In_User()
    {
        var user = new User();
        user.RecentlyUsedRelationTargetTopics = "";
        var userRepository = Resolve<UserRepo>();
        userRepository.Create(user);

        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo(""));

        RecentlyUsedRelationTargets.Add(3, user.Id);
        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("3"));

        RecentlyUsedRelationTargets.Add(6, user.Id);
        RecentlyUsedRelationTargets.Add(12, user.Id);
        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("3,6,12"));

        RecentlyUsedRelationTargets.Add(4, user.Id);
        Assert.That(userRepository.GetAll().FirstOrDefault().RecentlyUsedRelationTargetTopics, Is.EqualTo("6,12,4"));
    }
}