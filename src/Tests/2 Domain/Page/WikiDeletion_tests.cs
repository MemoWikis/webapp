// This class contains tests for the wiki deletion functionality.
// It covers various scenarios such as deleting wikis with and without children,
// handling permissions, and ensuring correct redirection after deletion.

internal class WikiDeletionTests : BaseTestHarness
{
    /// <summary>
    /// Verifies that a user can successfully delete a wiki if they own multiple wikis.
    /// The operation should succeed and not return any error messages.
    /// </summary>
    [Test]
    [Description("Should successfully delete a wiki when user has multiple wikis")]
    public async Task Should_delete_wiki_when_user_has_multiple_wikis()
    {
        await ClearData();

        // Arrange: Create a user with three wikis.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage.AddAndGet("First Wiki", creator, isWiki: true);
        var secondWiki = contextPage.AddAndGet("Second Wiki", creator, isWiki: true);
        var thirdWiki = contextPage.AddAndGet("Third Wiki", creator, isWiki: true);
        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act: Delete one of the wikis.
        var deleteResult = pageDeleter.DeletePage(secondWiki.Id, newParentForQuestionsId: null);

        // Assert: The deletion should be successful.
        await ReloadCaches();

        await Verify(new
        {
            deleteResult,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that a user cannot delete their last remaining wiki.
    /// This is a critical safety check to prevent data loss and ensure the user always has at least one wiki.
    /// </summary>
    [Test]
    [Description("Should fail to delete wiki when user has only one wiki")]
    public async Task Should_fail_to_delete_last_wiki()
    {
        await ClearData();

        // Arrange: Create a user with only one wiki.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var onlyWiki = contextPage.AddAndGet("Only Wiki", creator, isWiki: true);
        contextPage.Persist();
        await ReloadCaches();

        var cachedWiki = EntityCache.GetPage(onlyWiki);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedWiki!);

        var pageDeleter = R<PageDeleter>();

        // Act: Attempt to delete the only wiki.
        var deleteResult = pageDeleter.DeletePage(onlyWiki.Id, null);

        // Assert: The deletion should fail with a specific error message.
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiDiagram(cachedWiki!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that after deleting one of two wikis, the last remaining wiki cannot be deleted.
    /// </summary>
    [Test]
    [Description("Should fail to delete wiki when user has only two wikis but trying to delete both")]
    public async Task Should_fail_to_delete_second_to_last_wiki()
    {
        await ClearData();

        // Arrange: Create a user with two wikis.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage.AddAndGet("First Wiki", creator, isWiki: true);
        var secondWiki = contextPage.AddAndGet("Second Wiki", creator, isWiki: true);
        contextPage.Persist();
        await ReloadCaches();


        var pageDeleter = R<PageDeleter>();

        // Act: Delete the first wiki, then attempt to delete the second.
        var firstDeleteResult = pageDeleter.DeletePage(firstWiki.Id, null);
        var secondDeleteResult = pageDeleter.DeletePage(secondWiki.Id, null);

        // Assert: The first deletion should succeed, but the second should fail.
        await ReloadCaches();

        await Verify(new
        {
            firstDeleteResult,
            secondDeleteResult,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that a wiki can be deleted if its child pages have alternative parents.
    /// This ensures that no pages are orphaned by the deletion.
    /// </summary>
    [Test]
    [Description("Should successfully delete wiki with children that have alternative parents")]
    public async Task Should_delete_wiki_with_children_that_have_alternative_parents()
    {
        await ClearData();

        // Arrange: Create a wiki with a child page that also has another parent.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage.AddAndGet("First Wiki", creator, isWiki: true);
        var wikiToDelete = contextPage.AddAndGet("Wiki To Delete", creator, isWiki: true);
        var alternativeParent = contextPage.AddAndGet("Alternative Parent", creator);
        var childPage = contextPage.AddAndGet("Child Page", creator);
        contextPage.Persist();
        // Assign the child page to both the wiki being deleted and an alternative parent.
        contextPage.AddChild(firstWiki, alternativeParent);
        contextPage.AddChild(wikiToDelete, childPage);
        contextPage.AddChild(alternativeParent, childPage);

        await ReloadCaches();

        var cachedChildPage = EntityCache.GetPage(childPage);
        var originalTree = TreeRenderer.ToAsciiParentDiagram(cachedChildPage!);

        var pageDeleter = R<PageDeleter>();

        // Act: Delete the wiki.
        var deleteResult = pageDeleter.DeletePage(wikiToDelete.Id, null);

        // Assert: The deletion should succeed because the child is not orphaned.
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiParentDiagram(cachedChildPage!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that deleting a wiki fails if it would result in orphaned child pages.
    /// An orphaned page is one with no remaining parents.
    /// </summary>
    [Test]
    [Description("Should fail to delete wiki with orphaned children even when user has multiple wikis")]
    public async Task Should_fail_to_delete_wiki_with_orphaned_children()
    {
        await ClearData();

        // Arrange: Create a wiki with a child that has no other parents.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage.AddAndGet("First Wiki", creator, isWiki: true);
        var wikiToDelete = contextPage.AddAndGet("Wiki To Delete", creator, isWiki: true);
        var orphanedChild = contextPage.AddAndGet("Orphaned Child", creator);
        contextPage.Persist();
        contextPage.AddChild(wikiToDelete, orphanedChild);
        await ReloadCaches();

        var cachedChild = EntityCache.GetPage(orphanedChild);
        var originalTree = TreeRenderer.ToAsciiParentDiagram(cachedChild!);

        var pageDeleter = R<PageDeleter>();

        // Act: Attempt to delete the wiki.
        var deleteResult = pageDeleter.DeletePage(wikiToDelete.Id, null);

        // Assert: The deletion should fail, indicating there are children that would be orphaned.
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiParentDiagram(cachedChild!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }


    /// <summary>
    /// Verifies that a user cannot delete a wiki belonging to another user.
    /// This test ensures that page deletion respects ownership and permissions.
    /// </summary>
    [Test]
    [Description("Should fail to delete another user's wiki")]
    public async Task Should_fail_to_delete_another_users_wiki()
    {
        await ClearData();

        // Arrange: Create two users, each with their own wiki.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var currentUser = new User { Id = sessionUser.UserId };
        var otherUser = new User { Id = 999, Name = "Other User" };

        var otherUserContext = contextPage.ContextUser
            .Add(otherUser)
            .Persist()
            .GetUser("Other User");

        var userWiki = contextPage.AddAndGet("Current User's Wiki", currentUser, isWiki: true);
        var otherUsersWiki = contextPage.AddAndGet("Other User's Wiki", otherUserContext, isWiki: true);
        contextPage.Persist();
        await ReloadCaches();

        var cachedUserWiki = EntityCache.GetPage(userWiki);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedUserWiki!);

        var pageDeleter = R<PageDeleter>();

        // Act: The current user attempts to delete the other user's wiki.
        var deleteResult = pageDeleter.DeletePage(otherUsersWiki.Id, null);

        // Assert: The deletion should fail with a "no rights" error message.
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiDiagram(cachedUserWiki!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that a page without any parents, which is not explicitly a wiki, can be deleted.
    /// Such pages are considered deletable as they don't affect the wiki structure.
    /// </summary>
    [Test]
    public async Task Should_delete_parentless_page_that_is_not_explicit_wiki()
    {
        await ClearData();

        // Arrange: Create a parentless page that is not a wiki.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var wiki = contextPage.AddAndGet("Explicit Wiki", creator, isWiki: true);
        var orphanedPage = contextPage.AddAndGet("Parentless Page", creator, isWiki: false);

        contextPage.Persist();
        await ReloadCaches();

        var cachedWiki = EntityCache.GetPage(wiki);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedWiki!);

        var pageDeleter = R<PageDeleter>();

        // Act: Delete the parentless page.
        var deleteResult = pageDeleter.DeletePage(orphanedPage.Id, null);

        // Assert: The deletion should be successful.
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiDiagram(cachedWiki!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that a parentless page can be deleted, even if the user has other a wiki.
    /// The presence of other wikis should not prevent the deletion of a non-wiki, parentless page.
    /// </summary>
    [Test]
    public async Task Should_delete_parentless_page_when_user_has_wiki()
    {
        await ClearData();

        // Arrange: Create a user with an explicit wiki and a separate parentless page.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var wiki = contextPage.AddAndGet("Explicit Wiki", creator, isWiki: true);
        var orphanedPage = contextPage.AddAndGet("Parentless Page", creator, isWiki: false);

        contextPage.Persist();
        await ReloadCaches();

        var cachedWiki = EntityCache.GetPage(wiki);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedWiki!);

        var pageDeleter = R<PageDeleter>();

        // Act: Delete the parentless page.
        var deleteResult = pageDeleter.DeletePage(orphanedPage.Id, null);

        // Assert: The deletion should be successful.
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiDiagram(cachedWiki!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that after deleting a wiki, the user is redirected to the first available wiki.
    /// This test checks the specific scenario of deleting the second wiki in a list of three.
    /// </summary>
    [Test]
    public async Task Should_redirect_to_first_wiki_when_deleting_second_wiki()
    {
        await ClearData();

        // Arrange: Create a user with three wikis in a specific order.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage.AddAndGet("Alpha Wiki", creator, isWiki: true);
        var secondWiki = contextPage.AddAndGet("Beta Wiki", creator, isWiki: true);
        var thirdWiki = contextPage.AddAndGet("Gamma Wiki", creator, isWiki: true);
        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act: Delete the second wiki.
        var deleteResult = pageDeleter.DeletePage(secondWiki.Id, null);

        // Assert: The user should be redirected to the first wiki ("Alpha Wiki").
        await ReloadCaches();

        await Verify(new
        {
            deleteResult,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    /// <summary>
    /// Verifies that after deleting the first wiki in a list, the user is redirected to the next one.
    /// </summary>
    [Test]
    public async Task Should_redirect_to_second_wiki_when_deleting_first_wiki()
    {
        await ClearData();

        // Arrange: Create a user with three wikis.
        var contextPage = NewPageContext();
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var firstWiki = contextPage.AddAndGet("Alpha Wiki", creator, isWiki: true);
        var secondWiki = contextPage.AddAndGet("Beta Wiki", creator, isWiki: true);
        var thirdWiki = contextPage.AddAndGet("Gamma Wiki", creator, isWiki: true);
        contextPage.Persist();
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act: Delete the first wiki.
        var deleteResult = pageDeleter.DeletePage(firstWiki.Id, null);

        // Assert: The user should be redirected to the second wiki ("Beta Wiki").
        await ReloadCaches();

        await Verify(new
        {
            deleteResult,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }
}