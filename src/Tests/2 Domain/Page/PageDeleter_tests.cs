using Meilisearch;

internal class PageDeleter_tests : BaseTestHarness
{
    // Creating root wiki explicitly is necessary because:
    // 1. PermissionCheck prevents deletion of page with ID matching FeaturedPage.RootPageId
    // 2. When auto-creating pages in tests, a page might get the same ID as RootPageId
    // 3. Such a page would be undeletable (even by admins/creators), causing test issues

    [Test]
    public async Task Should_delete_child()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);

        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);

        //Assert
        Assert.That(requestResult.Success);
        Assert.That(requestResult.HasChildren, Is.False);
        Assert.That(requestResult.MessageKey, Is.Null);
        Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
    }

    [Test]
    public async Task Should_delete_child_of_child_and_remove_relation()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);

        await ReloadCaches();
        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(childOfChild.Id, parent.Id);

        //Assert
        await ReloadCaches();

        var pageRepo = R<PageRepository>();
        var allAvailablePages = pageRepo.GetAll();
        var parentChildren = pageRepo.GetChildren(parent.Id);
        var childrenOfChild = pageRepo.GetChildren(child.Id);

        Assert.That(requestResult, Is.Not.False);
        Assert.That(requestResult.Success);
        Assert.That(requestResult.HasChildren, Is.False);
        Assert.That(requestResult.MessageKey, Is.Null);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.That(allAvailablePages.Any());
        Assert.That(allAvailablePages.Contains(parent));
        Assert.That(allAvailablePages.Contains(child));
        Assert.That(parentChildren, Is.Not.Empty);
        Assert.That(parentChildren.Count == 1);
        Assert.That(childName, Is.EqualTo(parentChildren.First().Name));
        Assert.That(childrenOfChild, Is.Empty);
    }

    [Test]
    public async Task Should_delete_child_of_child_and_remove_relations_in_EntityCache()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);
        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(childOfChild.Id, parent.Id);
        await ReloadCaches();

        //Assert
        var allPagesInEntityCache = EntityCache.GetAllPagesList();
        var cachedParent = EntityCache.GetPage(parent.Id);
        var cachedChild = EntityCache.GetPage(child.Id);

        Assert.That(requestResult.Success);
        Assert.That(requestResult.HasChildren, Is.False);
        Assert.That(requestResult.MessageKey, Is.Null);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.That(allPagesInEntityCache.Any());
        Assert.That(allPagesInEntityCache.Any(c => c.Id == parent.Id));
        Assert.That(allPagesInEntityCache.Any(c => c.Id == child.Id));
        Assert.That(allPagesInEntityCache.Any(c => c.Name.Equals(childOfChildName)), Is.False);
        Assert.That(cachedParent.ChildRelations, Is.Not.Empty);
        Assert.That(cachedChild.Id,
            Is.EqualTo(cachedParent.ChildRelations.Single().ChildId));
        Assert.That(cachedParent.ParentRelations, Is.Empty);
        Assert.That(cachedChild.ChildRelations, Is.Empty);
        Assert.That(cachedParent.Id, Is.EqualTo(cachedChild.ParentRelations.Single().Id));

        var allRelationsInEntityCache = EntityCache.GetAllRelations();
        Assert.That(allRelationsInEntityCache.Any(r => r.ChildId == childOfChild.Id), Is.False);
    }

    [Test]
    [Description("child of child has extra parent")]
    public async Task Should_delete_child_and_remove_relations_in_EntityCache_child_of_child_has_extra_parent()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var firstChildName = "first child name";
        var secondChildName = "second child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var firstChild = contextPage
            .Add(firstChildName, creator)
            .GetPageByName(firstChildName);

        var secondChild = contextPage
            .Add(secondChildName, creator)
            .GetPageByName(secondChildName);

        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, firstChild);
        contextPage.AddChild(parent, secondChild);
        contextPage.AddChild(firstChild, childOfChild);
        contextPage.AddChild(secondChild, childOfChild);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(firstChild.Id, parent.Id);
        await ReloadCaches();

        //Assert
        Assert.That(requestResult.Success);
    }

    [Test]
    [Description("child has a child, so it can't be deleted or removed")]
    public async Task Should_fail_delete_child_and_remove_relations_in_EntityCache_child_has_child()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);
        await ReloadCaches();

        //Assert
        Assert.That(requestResult.Success, Is.False);
        Assert.That(requestResult.HasChildren);
        Assert.That(requestResult.MessageKey, Is.Null);
    }

    [Test]
    [Description("no rights")]
    public async Task Should_fail_delete_child_and_remove_relations_in_EntityCache_no_rights()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";

        var creator = new User { Id = 2, IsInstallationAdmin = false, Name = "Creator" };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        await ReloadCaches();

        //Act
        var pageDeleter = R<PageDeleter>();
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);


        //Assert
        await ReloadCaches();

        await Verify(requestResult);
    }

    [Test]
    [Description("Verify page is removed from Meilisearch after deletion")]
    public async Task Should_delete_page_from_meilisearch()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var pageName = "meilisearch test page";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var root = contextPage
            .Add("root", creator, isWiki: true)
            .GetPageByName("root");

        var page = contextPage
            .Add(pageName, creator, isWiki: true)
            .GetPageByName(pageName);

        contextPage.Persist();
        await ReloadCaches();

        // Create a client to directly verify Meilisearch state
        var client = new MeilisearchClient(_testHarness.MeilisearchUrl, TestHarness.MeilisearchMasterKey);
        var index = client.Index(MeilisearchIndices.Pages);

        // Allow time for initial indexing
        await Task.Delay(500);

        // Search for the page before deletion
        var beforeQuery = new SearchQuery { Q = pageName };
        var beforeResult = await index.SearchAsync<MeilisearchPageMap>(beforeQuery.ToString());

        // Act
        var pageDeleter = R<PageDeleter>();
        var deleteResult = pageDeleter.DeletePage(page.Id, null);

        // Allow time for deletion to propagate
        await Task.Delay(500);

        // Search for the page after deletion
        var afterQuery = new SearchQuery { Q = pageName };
        var afterResult = await index.SearchAsync<MeilisearchPageMap>(afterQuery.ToString());

        // Verify the before and after states
        await Verify(new
        {
            Page = new { Id = page.Id, Name = pageName },
            BeforeDelete =
                new
                {
                    ResultCount = beforeResult.Hits.Count,
                    ContainsPage = beforeResult.Hits.Any(p => p.Id == page.Id)
                },
            DeleteResult = deleteResult,
            AfterDelete = new
            {
                ResultCount = afterResult.Hits.Count,
                ContainsPage = afterResult.Hits.Any(p => p.Id == page.Id)
            }
        });
    }

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
    [Description("Should fail to delete root page with no rights error")]
    public async Task Should_fail_to_delete_root_page()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var pageDeleter = R<PageDeleter>();
        var rootPageId = FeaturedPage.RootPageId;

        var rootPage = contextPage
            .Add("Root Page")
            .GetPageByName("Root Page");

        contextPage.Persist();
        await ReloadCaches();

        // Act
        var result = pageDeleter.DeletePage(rootPageId, null);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.MessageKey, Is.EqualTo(FrontendMessageKeys.Error.Page.NoRights));
        Assert.That(result.RedirectParent, Is.Null);
    }

    [Test]
    [Description("Should redirect to first remaining wiki when deleting a wiki")]
    public async Task Should_redirect_to_first_remaining_wiki_when_deleting_wiki()
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
    [Description("Should redirect to first remaining wiki when deleting the first wiki")]
    public async Task Should_redirect_to_first_remaining_wiki_when_deleting_first_wiki()
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

    [Test]
    public async Task HandleQuestions_Should_Succeed_When_Valid_Parent_Provided()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var parentName = "parent page";
        var childName = "child page";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);

        // Add questions to the child page
        var questionContext = NewQuestionContext(persistImmediately: true);
        questionContext.AddQuestion("Test Question 1", creator: creator, pages: new List<Page> { child });
        questionContext.AddQuestion("Test Question 2", creator: creator, pages: new List<Page> { child });

        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act
        var result = pageDeleter.DeletePage(child.Id, parent.Id);

        // Assert
        await ReloadCaches();
        var questionsInParent = EntityCache.GetQuestionsForPage(parent.Id);

        await Verify(new
        {
            Success = result.Success,
            MessageKey = result.MessageKey,
            HasChildren = result.HasChildren,
            QuestionsMovedToParentCount = questionsInParent.Count,
            QuestionTexts = questionsInParent.Select(q => q.Text).OrderBy(t => t).ToList(),
            ParentId = parent.Id,
            ResultRedirectParentId = result.RedirectParent?.Id
        });
    }

    [Test]
    public async Task HandleQuestions_Should_Fail_When_No_Parent_Provided_For_Questions()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var childName = "child page with questions";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        contextPage.Persist();

        // Add questions to the child page
        var questionContext = NewQuestionContext(persistImmediately: true);
        questionContext.AddQuestion("Test Question", creator: creator, pages: new List<Page> { child });

        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Provide null parent for questions
        var result = pageDeleter.DeletePage(child.Id, null);

        // Assert
        await ReloadCaches();
        var childPage = EntityCache.GetPage(child.Id);
        var questionsInChild = EntityCache.GetQuestionsForPage(child.Id);

        await Verify(new
        {
            Success = result.Success,
            MessageKey = result.MessageKey,
            HasChildren = result.HasChildren,
            PageStillExists = childPage != null,
            QuestionsInChildCount = questionsInChild.Count,
            QuestionText = questionsInChild.FirstOrDefault()?.Text,
            ChildPageId = child.Id
        });
    }
}