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

        var root = contextPage.All.First();

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
        contextPage.AddChild(root, parent);
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);
        await ReloadCaches();

        var cachedRoot = EntityCache.GetPage(root);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);

        var pageDeleter = R<PageDeleter>();

        //Act
        var deleteResult = pageDeleter.DeletePage(childOfChild.Id, parent.Id);
        await ReloadCaches(); //Assert

        var newTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
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
        var cachedRoot = EntityCache.GetPage(parent);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);

        var pageDeleter = R<PageDeleter>();

        //Act
        var deleteResult = pageDeleter.DeletePage(firstPage.Id, parent.Id);
        await ReloadCaches(); //Assert
        var newTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
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

        var cachedParent = EntityCache.GetPage(parent);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedParent!);

        var pageDeleter = R<PageDeleter>();

        //Act
        var deleteResult = pageDeleter.DeletePage(page.Id, parent.Id);
        await ReloadCaches();

        //Assert
        var newTree = TreeRenderer.ToAsciiDiagram(cachedParent!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
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

        var cachedParent = EntityCache.GetPage(parent);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedParent!);

        // Act: Attempt to delete the child page as the session user.
        // The PageDeleter service operates on behalf of the session user (user ID 1),
        // who does not have permission to delete the page.
        var pageDeleter = R<PageDeleter>();
        var deleteResult = pageDeleter.DeletePage(child.Id, parent.Id);

        // Assert: The deletion attempt should fail.
        await ReloadCaches();

        var newTree = TreeRenderer.ToAsciiDiagram(cachedParent!);

        // Verify that the result of the operation indicates a permission error.
        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    [Test]
    [Description("Should fail to delete root page with no rights error")]
    public async Task Should_fail_to_delete_root_page()
    {
        await ClearData(); // Arrange
        var contextPage = NewPageContext();
        var pageDeleter = R<PageDeleter>();
        var rootPageId = FeaturedPage.RootPageId;

        contextPage.Persist();
        await ReloadCaches();

        var cachedRoot = EntityCache.GetPage(rootPageId);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);

        // Act
        // Root page is already created in NewPageContext()
        var deleteResult = pageDeleter.DeletePage(rootPageId, null);

        // Assert
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);

        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    [Test]
    public async Task Should_Succeed_When_Valid_Parent_Provided()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
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
        var cachedRoot = EntityCache.GetPage(parent);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);

        var pageDeleter = R<PageDeleter>(); // Act
        var deleteResult = pageDeleter.DeletePage(child.Id, parent.Id);

        // Assert
        await ReloadCaches();
        var newQuestionsInParent = EntityCache.GetQuestionsForPage(parent.Id);

        var newTree = TreeRenderer.ToAsciiDiagram(cachedRoot!);
        await Verify(new
        {
            deleteResult,
            originalTree,
            newTree,
            newQuestionsInParent,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }

    [Test]
    public async Task Should_Fail_When_No_Parent_Provided_For_Questions()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
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

        var pageDeleter = R<PageDeleter>(); // Act - Provide null parent for questions
        var deleteResult = pageDeleter.DeletePage(child.Id, null);

        // Assert
        await ReloadCaches();

        await Verify(new
        {
            deleteResult,
            PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
        });
    }
}