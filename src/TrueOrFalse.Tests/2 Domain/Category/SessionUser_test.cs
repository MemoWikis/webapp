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

        var beforeSettingId = SessionUser.CurrentWikiId;
        SessionUser.SetWikiId(category3);

        Assert.That(beforeSettingId, Is.EqualTo(1));
        Assert.That(SessionUser.CurrentWikiId, Is.EqualTo(3));
    }
}
