internal class WikiDeletionTests : BaseTestHarness
{
    [Test]
    [Description("Should successfully delete a wiki when user has multiple wikis")]
    public async Task Should_delete_wiki_when_user_has_multiple_wikis()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage
            .Add("First Wiki", creator, isWiki: true)
            .GetPageByName("First Wiki");

        var secondWiki = contextPage
            .Add("Second Wiki", creator, isWiki: true)
            .GetPageByName("Second Wiki");

        var thirdWiki = contextPage
            .Add("Third Wiki", creator, isWiki: true)
            .GetPageByName("Third Wiki");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act
        var result = pageDeleter.DeletePage(secondWiki.Id, null);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.HasChildren, Is.False);
        Assert.That(result.MessageKey, Is.Null);
        Assert.That(result.RedirectParent, Is.Not.Null);
    }

    [Test]
    [Description("Should fail to delete wiki when user has only one wiki")]
    public async Task Should_fail_to_delete_last_wiki()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var onlyWiki = contextPage
            .Add("Only Wiki", creator, isWiki: true)
            .GetPageByName("Only Wiki");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act
        var result = pageDeleter.DeletePage(onlyWiki.Id, null);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.MessageKey, Is.EqualTo(FrontendMessageKeys.Error.User.NoRemainingWikis));
        Assert.That(result.RedirectParent, Is.Null);
    }

    [Test]
    [Description("Should fail to delete wiki when user has only two wikis but trying to delete both")]
    public async Task Should_fail_to_delete_second_to_last_wiki()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage
            .Add("First Wiki", creator, isWiki: true)
            .GetPageByName("First Wiki");

        var secondWiki = contextPage
            .Add("Second Wiki", creator, isWiki: true)
            .GetPageByName("Second Wiki");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Delete first wiki (should succeed)
        var firstResult = pageDeleter.DeletePage(firstWiki.Id, null);

        // Act - Try to delete last remaining wiki (should fail)
        var secondResult = pageDeleter.DeletePage(secondWiki.Id, null);

        // Assert
        Assert.That(firstResult.Success, Is.True);
        Assert.That(secondResult.Success, Is.False);
        Assert.That(secondResult.MessageKey, Is.EqualTo(FrontendMessageKeys.Error.User.NoRemainingWikis));
        Assert.That(secondResult.RedirectParent, Is.Null);
    }

    [Test]
    [Description("Should successfully delete wiki with children that have alternative parents")]
    public async Task Should_delete_wiki_with_children_that_have_alternative_parents()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage
            .Add("First Wiki", creator, isWiki: true)
            .GetPageByName("First Wiki");

        var wikiToDelete = contextPage
            .Add("Wiki To Delete", creator, isWiki: true)
            .GetPageByName("Wiki To Delete");

        var alternativeParent = contextPage
            .Add("Alternative Parent", creator)
            .GetPageByName("Alternative Parent");

        var childPage = contextPage
            .Add("Child Page", creator)
            .GetPageByName("Child Page");

        contextPage.Persist();

        // Give the child page TWO parents - the wiki to delete AND an alternative parent
        contextPage.AddChild(wikiToDelete, childPage);
        contextPage.AddChild(alternativeParent, childPage);

        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Should succeed because child has alternative parent
        var result = pageDeleter.DeletePage(wikiToDelete.Id, null);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.HasChildren, Is.False);
        Assert.That(result.MessageKey, Is.Null);
        Assert.That(result.RedirectParent, Is.Not.Null);
    }

    [Test]
    [Description("Should fail to delete wiki with orphaned children even when user has multiple wikis")]
    public async Task Should_fail_to_delete_wiki_with_orphaned_children()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage
            .Add("First Wiki", creator, isWiki: true)
            .GetPageByName("First Wiki");

        var wikiToDelete = contextPage
            .Add("Wiki To Delete", creator, isWiki: true)
            .GetPageByName("Wiki To Delete");

        var orphanedChild = contextPage
            .Add("Orphaned Child", creator)
            .GetPageByName("Orphaned Child");

        contextPage.Persist();
        contextPage.AddChild(wikiToDelete, orphanedChild);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act
        var result = pageDeleter.DeletePage(wikiToDelete.Id, null);

        // Assert - This should fail because the child would become orphaned
        Assert.That(result.Success, Is.False);
        Assert.That(result.HasChildren, Is.True);
        Assert.That(result.RedirectParent, Is.Null);
    }

    [Test]
    [Description("Should fail to delete another user's wiki")]
    public async Task Should_fail_to_delete_another_users_wiki()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var currentUser = new User { Id = sessionUser.UserId };
        var otherUser = new User { Id = 999, Name = "Other User" };

        // Create another user using the page context's user context
        var otherUserContext = contextPage.ContextUser
            .Add(otherUser)
            .Persist()
            .GetUser("Other User");

        var userWiki = contextPage
            .Add("Current User's Wiki", currentUser, isWiki: true)
            .GetPageByName("Current User's Wiki");

        var otherUsersWiki = contextPage
            .Add("Other User's Wiki", otherUserContext, isWiki: true)
            .GetPageByName("Other User's Wiki");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act
        var result = pageDeleter.DeletePage(otherUsersWiki.Id, null);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.MessageKey, Is.EqualTo(FrontendMessageKeys.Error.Page.NoRights));
        Assert.That(result.RedirectParent, Is.Null);
    }

    [Test]
    [Description("Should allow deletion of parentless page that is not an explicit wiki")]
    public async Task Should_delete_parentless_page_that_is_not_explicit_wiki()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var explicitWiki = contextPage
            .Add("Explicit Wiki", creator, isWiki: true)
            .GetPageByName("Explicit Wiki");

        // Create a page without explicitly setting isWiki=true, and it has no parents
        // This is not an explicit wiki, so it can be deleted
        var parentlessPage = contextPage
            .Add("Parentless Page", creator, isWiki: false)
            .GetPageByName("Parentless Page");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act
        var result = pageDeleter.DeletePage(parentlessPage.Id, null);

        // Assert - Should succeed since it's not an explicit wiki
        Assert.That(result.Success, Is.True);
        Assert.That(result.MessageKey, Is.Null);
        Assert.That(result.RedirectParent, Is.Not.Null);
    }

    [Test]
    [Description("Should allow deletion of parentless page when user has explicit wikis")]
    public async Task Should_delete_parentless_page_when_user_has_explicit_wikis()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        // Create an explicit wiki
        var explicitWiki = contextPage
            .Add("Explicit Wiki", creator, isWiki: true)
            .GetPageByName("Explicit Wiki");

        // Create a parentless page (wiki type but not explicit wiki)
        var parentlessPage = contextPage
            .Add("Parentless Page", creator, isWiki: false)
            .GetPageByName("Parentless Page");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Delete the parentless page (should succeed since user has explicit wiki)
        var result = pageDeleter.DeletePage(parentlessPage.Id, null);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.MessageKey, Is.Null);
        Assert.That(result.RedirectParent, Is.Not.Null);
    }

    [Test]
    public async Task Should_redirect_to_first_wiki_when_deleting_second_wiki()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage
            .Add("Alpha Wiki", creator, isWiki: true)
            .GetPageByName("Alpha Wiki");

        var secondWiki = contextPage
            .Add("Beta Wiki", creator, isWiki: true)
            .GetPageByName("Beta Wiki");

        var thirdWiki = contextPage
            .Add("Gamma Wiki", creator, isWiki: true)
            .GetPageByName("Gamma Wiki");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Delete the second wiki
        var result = pageDeleter.DeletePage(secondWiki.Id, null);

        // Assert - Should redirect to the first wiki (Alpha Wiki)
        await Verify(new
        {
            Success = result.Success,
            HasChildren = result.HasChildren,
            MessageKey = result.MessageKey,
            RedirectParent = result.RedirectParent
        });
    }

    [Test]
    public async Task Should_redirect_to_second_wiki_when_deleting_first_wiki()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage
            .Add("Alpha Wiki", creator, isWiki: true)
            .GetPageByName("Alpha Wiki");

        var secondWiki = contextPage
            .Add("Beta Wiki", creator, isWiki: true)
            .GetPageByName("Beta Wiki");

        var thirdWiki = contextPage
            .Add("Gamma Wiki", creator, isWiki: true)
            .GetPageByName("Gamma Wiki");

        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Delete the first wiki (Alpha Wiki)
        var result = pageDeleter.DeletePage(firstWiki.Id, null);

        // Assert - Should redirect to the first remaining wiki (Beta Wiki)
        await Verify(new
        {
            Success = result.Success,
            HasChildren = result.HasChildren,
            MessageKey = result.MessageKey,
            RedirectParent = result.RedirectParent
        });
    }
}