internal class PageDeleter_tests : BaseTestHarness
{
    // ToDo: Verify Searchindex, EntityCache and Database
    // Treebuilder: Before and After Act
    // Keep Verify objects small, (use filters)
    // Verify:
    // Id, Relations, Page

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
        Assert.That(allPagesInEntityCache.Any(page => page.Id == parent.Id));
        Assert.That(allPagesInEntityCache.Any(page => page.Id == child.Id));
        Assert.That(allPagesInEntityCache.Any(page => page.Name.Equals(childOfChildName)), Is.False);
        Assert.That(cachedParent.ChildRelations, Is.Not.Empty);
        Assert.That(cachedChild.Id,
            Is.EqualTo(cachedParent.ChildRelations.Single().ChildId));
        Assert.That(cachedParent.ParentRelations, Is.Empty);
        Assert.That(cachedChild.ChildRelations, Is.Empty);
        Assert.That(cachedParent.Id, Is.EqualTo(cachedChild.ParentRelations.Single().Id));

        var allRelationsInEntityCache = EntityCache.GetAllRelations();
        Assert.That(allRelationsInEntityCache.Any(r => r.ChildId == childOfChild.Id), Is.False);

        //ToDo: use Verify
    }

    [Test]
    public async Task Should_delete_page_with_child_with_multiple_parents()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var firstPageName = "first child name";
        var secondPageName = "second child name";
        var childWidthTwoParentsName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var firstPage = contextPage
            .Add(firstPageName, creator)
            .GetPageByName(firstPageName);

        var secondPage = contextPage
            .Add(secondPageName, creator)
            .GetPageByName(secondPageName);

        var childWithTwoParents = contextPage
            .Add(childWidthTwoParentsName, creator)
            .GetPageByName(childWidthTwoParentsName);

        contextPage.Persist();
        contextPage.AddChild(parent, firstPage);
        contextPage.AddChild(parent, secondPage);
        contextPage.AddChild(firstPage, childWithTwoParents);
        contextPage.AddChild(secondPage, childWithTwoParents);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(firstPage.Id, parent.Id);
        await ReloadCaches();

        //Assert
        Assert.That(requestResult.Success);

        //ToDo: use treebuilder and verify
    }

    [Test]
    public async Task Should_fail_delete_page_if_child_would_be_orphaned()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var pageName = "child name";
        var childOfPageName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var page = contextPage
            .Add(pageName, creator)
            .GetPageByName(pageName);

        var childOfPage = contextPage
            .Add(childOfPageName, creator)
            .GetPageByName(childOfPageName);

        contextPage.Persist();
        contextPage.AddChild(parent, page);
        contextPage.AddChild(page, childOfPage);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(page.Id, parent.Id);
        await ReloadCaches();

        //Assert
        Assert.That(requestResult.Success, Is.False);
        Assert.That(requestResult.HasChildren);
        Assert.That(requestResult.MessageKey, Is.Null);
    }

    /// <summary>
    /// This test verifies that a user cannot delete a page that they do not own.
    /// It checks that the correct "no rights" error is returned.
    /// </summary>
    [Test]
    public async Task Should_fail_delete_page_if_no_rights()
    {
        await ClearData();

        // Arrange: Create a parent and a child page owned by a specific user (creator).
        // This user is different from the currently logged-in session user.
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";

        var creator = new User { Id = 2, IsInstallationAdmin = false, Name = "Creator" };

        var parent = contextPage.AddAndGet(parentName, creator);
        var child = contextPage.AddAndGet(childName, creator);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        await ReloadCaches();

        // Act: Attempt to delete the child page as the session user.
        // The PageDeleter service operates on behalf of the session user (user ID 1),
        // who does not have permission to delete the page.
        var pageDeleter = R<PageDeleter>();
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);

        // Assert: The deletion attempt should fail.
        await ReloadCaches();

        // Verify that the result of the operation indicates a permission error.
        await Verify(requestResult);
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
    public async Task Should_Fail_When_No_Parent_Provided_For_Questions()
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
            ChildPageId = child.Id        });
    }
}