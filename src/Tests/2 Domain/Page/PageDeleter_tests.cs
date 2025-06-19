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
            ChildPageId = child.Id
        });
    }

    [Test]
    [Description("Test rapid page deletion to reproduce bulk manipulation query error")]
    public async Task Should_Handle_Rapid_Page_Deletion_Without_Database_Errors()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        // Create a parent page that will receive moved questions
        var parentPage = contextPage
            .Add("Parent Page", creator)
            .GetPageByName("Parent Page");

        // Create multiple child pages to simulate rapid deletion scenario
        var childPages = new List<Page>();

        for (int i = 1; i <= 5; i++)
        {
            var childPage = contextPage
                .Add($"Child Page {i}", creator)
                .GetPageByName($"Child Page {i}");

            childPages.Add(childPage);
        }

        contextPage.Persist();

        // Set up parent-child relationships
        foreach (var child in childPages)
        {
            contextPage.AddChild(parentPage, child);
        }

        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Simulate rapid deletion of multiple pages
        var deletionResults = new List<PageDeleter.DeletePageResult>();

        // Create tasks for rapid concurrent deletion
        foreach (var child in childPages)
        {
            // Use Task.Run to simulate concurrent execution
            await Task.Delay(100); // Small delay to simulate real-world timing

            try
            {
                deletionResults.Add(pageDeleter.DeletePage(child.Id, parentPage.Id));
            }
            catch (Exception ex)
            {
                deletionResults.Add(new PageDeleter.DeletePageResult(
                    HasChildren: false,
                    Success: false,
                    RedirectParent: null,
                    MessageKey: $"Exception: {ex.Message}"
                ));
            }
        }

        await Verify(new
        {
            ConcurrentDeletionResults =
                deletionResults.Select((r, index) => new
                {
                    PageIndex = index + 1,
                    Success = r.Success,
                    MessageKey = r.MessageKey,
                    HasChildren = r.HasChildren,
                    HasException = r.MessageKey?.Contains("Exception") == true
                }).ToList(),
            FinalState = new
            {
                ParentId = parentPage.Id,

                // Check if any pages still exist that should have been deleted
                PagesStillExisting = childPages
                    .Where(p => EntityCache.GetPage(p.Id) != null)
                    .Select(p => new { p.Id, p.Name })
                    .ToList(),

                // Summary of any errors encountered
                ErrorSummary = new
                {
                    ConcurrentErrors = deletionResults.Count(r => r.MessageKey?.Contains("Exception") == true).ToString(),
                }
            }
        });
    }

    [Test]
    [Description("Test rapid concurrent page deletion to reproduce bulk manipulation query error")]
    public async Task Should_Handle_Concurrent_Rapid_Page_Deletion_Without_Database_Errors()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        // Create a parent page that will receive moved questions
        var parentPage = contextPage
            .Add("Parent Page", creator)
            .GetPageByName("Parent Page");

        // Create multiple child pages to simulate rapid deletion scenario
        var childPages = new List<Page>();

        for (int i = 1; i <= 5; i++)
        {
            var childPage = contextPage
                .Add($"Child Page {i}", creator)
                .GetPageByName($"Child Page {i}");

            childPages.Add(childPage);
        }

        contextPage.Persist();

        // Set up parent-child relationships
        foreach (var child in childPages)
        {
            contextPage.AddChild(parentPage, child);
        }

        await ReloadCaches();

        // Act - Simulate rapid concurrent deletion using real API calls
        var parentPageId = parentPage.Id;
        var deletionTasks = childPages.Select(child => Task.Run(async () =>
        {
            var childId = child.Id;
            try
            {
                // Small delay to simulate rapid user clicks (but not simultaneous)
                await Task.Delay(Random.Shared.Next(10, 50));                // Make actual HTTP API call - this creates a real request with proper DI scope
                var requestBody = new { pageToDeleteId = childId, parentForQuestionsId = parentPageId };
                var jsonResponseContent = await _testHarness.ApiPost("/apiVue/DeletePageStore/Delete", requestBody);

                return new
                {
                    PageId = childId,
                    StatusCode = 200, // ApiPost only returns content on success
                    IsSuccess = true,
                    Content = jsonResponseContent,
                    HasException = false
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    PageId = childId,
                    StatusCode = 0,
                    IsSuccess = false,
                    Content = $"Exception: {ex.GetType().Name}: {ex.Message}\nStack Trace: {ex.StackTrace}",
                    HasException = true
                };
            }
        })).ToList();

        var results = await Task.WhenAll(deletionTasks);

        await Verify(new
        {
            ConcurrentDeletionResults = results.OrderBy(r => r.PageId).ToList(),
            FinalState = new
            {
                ParentId = parentPage.Id,

                // Check if any pages still exist that should have been deleted
                PagesStillExisting = childPages
                    .Where(p => EntityCache.GetPage(p.Id) != null)
                    .Select(p => new { p.Id, p.Name })
                    .ToList(),

                // Summary of any errors encountered
                ErrorSummary = new
                {
                    ConcurrentErrors = results.Count(r => r.HasException).ToString(),
                    HttpErrors = results.Count(r => !r.IsSuccess).ToString(),
                    TotalRequests = results.Length.ToString()
                }
            }
        });
    }

    [Test]
    [Description("Test slow concurrent page deletion to reproduce bulk manipulation query error")]
    public async Task Should_Handle_Slow_Page_Deletion_Without_Database_Errors_Concurrent()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        // Create a parent page that will receive moved questions
        var parentPage = contextPage
            .Add("Parent Page", creator)
            .GetPageByName("Parent Page");

        // Create multiple child pages to simulate rapid deletion scenario
        var childPages = new List<Page>();

        for (int i = 1; i <= 5; i++)
        {
            var childPage = contextPage
                .Add($"Child Page {i}", creator)
                .GetPageByName($"Child Page {i}");

            childPages.Add(childPage);
        }

        contextPage.Persist();

        // Set up parent-child relationships
        foreach (var child in childPages)
        {
            contextPage.AddChild(parentPage, child);
        }

        await ReloadCaches();

        // Act - Simulate rapid deletion using real API calls (sequential rapid clicks)
        var results = new List<dynamic>();

        // Capture necessary data
        var parentPageId = parentPage.Id;

        foreach (var child in childPages)
        {
            var childId = child.Id;            try
            {
                // Small delay to simulate rapid user clicks (but not simultaneous)
                await Task.Delay(Random.Shared.Next(1000, 2500));                // Make actual HTTP API call - this creates a real request with proper DI scope
                var requestBody = new { pageToDeleteId = childId, parentForQuestionsId = parentPageId };
                var jsonResponseContent = await _testHarness.ApiPost("/apiVue/DeletePageStore/Delete", requestBody);

                results.Add(new
                {
                    PageId = childId,
                    StatusCode = 200, // ApiPost only returns content on success
                    IsSuccess = true,
                    Content = jsonResponseContent,
                    HasException = false
                });
            }
            catch (Exception ex)
            {
                results.Add(new
                {
                    PageId = childId,
                    StatusCode = 0,
                    IsSuccess = false,
                    Content = $"Exception: {ex.GetType().Name}: {ex.Message}\nStack Trace: {ex.StackTrace}",
                    HasException = true
                });
            }
        }

        await Verify(new
        {
            SequentialDeletionResults = results,
            FinalState = new
            {
                ParentId = parentPage.Id,

                // Check if any pages still exist that should have been deleted
                PagesStillExisting = childPages
                    .Where(p => EntityCache.GetPage(p.Id) != null)
                    .Select(p => new { p.Id, p.Name })
                    .ToList(),

                // Summary of any errors encountered
                ErrorSummary = new
                {
                    ConcurrentErrors = results.Count(r => r.HasException).ToString(),
                    HttpErrors = results.Count(r => !r.IsSuccess).ToString(),
                    TotalRequests = results.Count.ToString()
                }
            }
        });
    }

    [Test]
    [Description("Test rapid concurrent page deletion with questions to reproduce bulk manipulation query error")]
    public async Task Should_Handle_Rapid_Page_With_Questions_Deletion_Without_Database_Errors()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext(createFeaturedRootPage: true);
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        // Create a parent page that will receive moved questions
        var parentPage = contextPage
            .Add("Parent Page", creator)
            .GetPageByName("Parent Page");

        // Create multiple child pages to simulate rapid deletion scenario
        var childPages = new List<Page>();
        var questionContext = NewQuestionContext();

        for (int i = 1; i <= 10; i++)
        {
            var childPage = contextPage
                .Add($"Child Page {i}", creator)
                .GetPageByName($"Child Page {i}");

            childPages.Add(childPage);

            // Add multiple questions to each page to increase the database load
            for (int j = 1; j <= 3; j++)
            {
                questionContext.AddQuestion($"Question {j} for Page {i}", creator: creator,
                    pages: new List<Page> { childPage });
            }
        }

        contextPage.Persist();
        questionContext.Persist();

        // Set up parent-child relationships
        foreach (var child in childPages)
        {
            contextPage.AddChild(parentPage, child);
        }

        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        // Act - Simulate rapid deletion of multiple pages with questions
        var deletionTasks = new List<Task<PageDeleter.DeletePageResult>>();
        var results = new List<PageDeleter.DeletePageResult>();

        // Capture necessary data in the main thread context to avoid DI container issues
        var pageDeleterInstance = pageDeleter;
        var parentPageId = parentPage.Id;

        foreach (var child in childPages)
        {
            var childId = child.Id; // Capture the child ID for the closure

            // Use Task.Run to simulate concurrent execution
            var task = Task.Run(() =>
            {
                try
                {
                    // Use the captured instances to avoid thread context issues
                    return pageDeleterInstance.DeletePage(childId, parentPageId);
                }
                catch (Exception ex)
                {
                    return new PageDeleter.DeletePageResult(
                        HasChildren: false,
                        Success: false,
                        RedirectParent: null,
                        MessageKey: $"Exception: {ex.Message}"
                    );
                }
            });
            deletionTasks.Add(task);
        }

        // Wait for all deletions to complete
        var taskResults = await Task.WhenAll(deletionTasks);
        results.AddRange(taskResults);

        // Assert
        await ReloadCaches();
        var questionsInParent = EntityCache.GetQuestionsForPage(parentPage.Id);

        await Verify(new
        {
            DeletionResults =
                results.Select((r, index) => new
                {
                    PageIndex = index + 1,
                    Success = r.Success,
                    MessageKey = r.MessageKey,
                    HasChildren = r.HasChildren,
                    HasException = r.MessageKey?.Contains("Exception") == true,
                    HasBulkManipulationError =
                        r.MessageKey?.Contains("could not execute native bulk manipulation query") == true
                }).ToList(),
            Summary = new
            {
                TotalPagesProcessed = results.Count.ToString(),
                SuccessfulDeletions = results.Count(r => r.Success).ToString(),
                FailedDeletions = results.Count(r => !r.Success).ToString(),
                ExceptionsThrown = results.Count(r => r.MessageKey?.Contains("Exception") == true),
                BulkManipulationErrors =
                    results.Count(r =>
                        r.MessageKey?.Contains("could not execute native bulk manipulation query") == true),
                FinalQuestionsInParent = questionsInParent.Count.ToString(),
                ExpectedQuestionsInParent = childPages.Count * 3, // 3 questions per page
                ParentId = parentPage.Id
            }
        });
    }
}