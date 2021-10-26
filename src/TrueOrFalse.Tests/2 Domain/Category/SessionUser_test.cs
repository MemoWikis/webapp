using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using GraphJsonDtos;
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

        var sessionUser = Sl.SessionUser;
        var beforeSettingId = sessionUser.CurrentWikiId;
        sessionUser.SetWikiId(category3);

        Assert.That(beforeSettingId, Is.EqualTo(1));
        Assert.That(sessionUser.CurrentWikiId, Is.EqualTo(3));
    }
}
