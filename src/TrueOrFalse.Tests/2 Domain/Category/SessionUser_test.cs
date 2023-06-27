using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class SessionUser_test : BaseTest
{
    [Test]
    public void Should_Set_WikiId()
    {
        var contextCategory = ContextCategory.New();

        contextCategory.Add(3);

        var categories = CategoryCacheItem.ToCacheCategories(contextCategory.All).ToList();

        var category3 = categories[2];

        var beforeSettingId = Resolve<SessionUser>().CurrentWikiId;
        Resolve<SessionUser>().SetWikiId(category3);

        Assert.That(beforeSettingId, Is.EqualTo(1));
        Assert.That(Resolve<SessionUser>().CurrentWikiId, Is.EqualTo(3));
    }
}
