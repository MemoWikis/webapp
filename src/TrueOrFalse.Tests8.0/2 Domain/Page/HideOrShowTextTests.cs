public class HideOrShowTextTests : BaseTest
{
    [Test]
    public void Ensure_TextIsHidden_consistency_between_cache_and_db()
    {
        //Arrange
        //visiblePage
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };
        var contextCategory = ContextPage.New(false);
        var publicPageName = "category1";
        var publicPage = contextCategory
            .Add(publicPageName, creator: creator)
            .Persist()
            .GetPageByName(publicPageName);

        //NotVisiblePage
        var creator1 = new User { Name = "Daniel" };
        ContextUser.New(R<UserWritingRepo>()).Add(creator1).Persist();
        var privatePageName = "category2";
        var privatePage = contextCategory
            .Add(privatePageName, creator: creator1, visibility: PageVisibility.Owner)
            .Persist()
            .GetPageByName(privatePageName);

        //Act
        var resultVisiblePage = R<PageUpdater>().HideOrShowPageText(hideText: true, publicPage.Id);

        var dbCategory = R<PageRepository>().GetById(publicPage.Id);
        var cacheCategory = EntityCache.GetPage(publicPage.Id);
        //Assert
        Assert.NotNull(dbCategory);
        Assert.NotNull(cacheCategory);
        Assert.True(resultVisiblePage);
        Assert.True(dbCategory.TextIsHidden);
        Assert.True(cacheCategory.TextIsHidden);

        var ex = Assert.Throws<AccessViolationException>(() => R<PageUpdater>().HideOrShowPageText(hideText: true, privatePage.Id));
        Assert.That(ex.Message, Is.EqualTo($"{nameof(PageUpdater.HideOrShowPageText)}: No permission for user"));
    }
}

