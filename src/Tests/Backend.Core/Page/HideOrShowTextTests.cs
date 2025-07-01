internal class HideOrShowTextTests : BaseTestHarness
{
    [Test]
    public void Ensure_TextIsHidden_consistency_between_cache_and_db()
    {
        //Arrange
        //visiblePage
        var creator = new User { Id = _testHarness.DefaultSessionUserId };
        var contextPage = NewPageContext();
        var publicPageName = "page1";
        var publicPage = contextPage
            .Add(publicPageName, creator: creator)
            .Persist()
            .GetPageByName(publicPageName);

        //NotVisiblePage
        var creator1 = new User { Name = "Daniel" };
        ContextUser.New(R<UserWritingRepo>()).Add(creator1).Persist();
        var privatePageName = "page2";
        var privatePage = contextPage
            .Add(privatePageName, creator: creator1, visibility: PageVisibility.Private)
            .Persist()
            .GetPageByName(privatePageName);

        //Act
        var resultVisiblePage = R<PageUpdater>().HideOrShowPageText(hideText: true, publicPage.Id);

        var dbPage = R<PageRepository>().GetById(publicPage.Id);
        var cachePage = EntityCache.GetPage(publicPage.Id);
        //Assert
        Assert.That(dbPage, Is.Not.Null);
        Assert.That(cachePage, Is.Not.Null);
        Assert.That(resultVisiblePage);
        Assert.That(dbPage.TextIsHidden);
        Assert.That(cachePage.TextIsHidden);

        var ex = Assert.Throws<AccessViolationException>(() => R<PageUpdater>().HideOrShowPageText(hideText: true, privatePage.Id));
        Assert.That(ex.Message, Is.EqualTo($"{nameof(PageUpdater.HideOrShowPageText)}: No permission for user"));
    }
}

