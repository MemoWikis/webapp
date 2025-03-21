﻿public class HideOrShowTextTests : BaseTest
{
    [Test]
    public void Ensure_TextIsHidden_consistency_between_cache_and_db()
    {
        //Arrange
        //visiblePage
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };
        var contextPage = ContextPage.New(false);
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
            .Add(privatePageName, creator: creator1, visibility: PageVisibility.Owner)
            .Persist()
            .GetPageByName(privatePageName);

        //Act
        var resultVisiblePage = R<PageUpdater>().HideOrShowPageText(hideText: true, publicPage.Id);

        var dbPage = R<PageRepository>().GetById(publicPage.Id);
        var cachePage = EntityCache.GetPage(publicPage.Id);
        //Assert
        Assert.NotNull(dbPage);
        Assert.NotNull(cachePage);
        Assert.True(resultVisiblePage);
        Assert.True(dbPage.TextIsHidden);
        Assert.True(cachePage.TextIsHidden);

        var ex = Assert.Throws<AccessViolationException>(() => R<PageUpdater>().HideOrShowPageText(hideText: true, privatePage.Id));
        Assert.That(ex.Message, Is.EqualTo($"{nameof(PageUpdater.HideOrShowPageText)}: No permission for user"));
    }
}

