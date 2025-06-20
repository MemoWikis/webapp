internal class PageDeleter_Technical_tests : BaseTestHarness
{
    /// <summary>
    /// Technical tests for testing performance, concurrency, and bulk operations
    /// to reproduce and verify database errors under load conditions.
    /// </summary>
    [Test]
    public async Task Should_Handle_Concurrent_Rapid_Page_Deletion_Without_Database_Errors()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
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
            contextPage.AddChild(parentPage, child);

        await ReloadCaches();

        // Act - Simulate rapid concurrent deletion using real API calls
        var parentPageId = parentPage.Id;
        var deletionTasks = childPages.Select(child => Task.Run(async () =>
        {
            var childId = child.Id;

            // Small delay to simulate rapid user clicks (but not simultaneous)
            await Task.Delay(Random.Shared.Next(10, 50));                // Make actual HTTP API call - this creates a real request with proper DI scope
            var requestBody = new { pageToDeleteId = childId, parentForQuestionsId = parentPageId };
            var result = await _testHarness.ApiPost<DeletePageStoreController.DeleteResponse>("/apiVue/DeletePageStore/Delete", requestBody);

            return result;
        })).ToList();

        var results = await Task.WhenAll(deletionTasks);

        await Verify(new
        {
            FinalState = new
            {
                ParentId = parentPage.Id,

                // Check if any pages still exist that should have been deleted
                PagesStillExisting = childPages
                    .Where(p => EntityCache.GetPage(p.Id) != null)
                    .Select(p => new { p.Id, p.Name })
                    .ToList(),
            }
        });
    }

    [Test]
    public async Task Should_Handle_Rapid_Page_With_Questions_Deletion_Without_Database_Errors()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
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
