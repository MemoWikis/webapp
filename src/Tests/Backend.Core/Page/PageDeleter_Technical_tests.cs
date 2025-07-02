internal class PageDeleter_Technical_tests : BaseTestHarness
{
    private UserLoginApiWrapper _userLoginApi => _testHarness.ApiUserLogin;

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
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        _userLoginApi.SetupSessionUserWiki(contextPage); // Ensure session user has a wiki
        // Create a parent page that will receive moved questions
        var parentPage = contextPage
            .Add("Parent Page", creator, isWiki: true)
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

        var cachedParent = EntityCache.GetPage(parentPage);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedParent!);

        // Act - Simulate rapid concurrent deletion using real API calls
        await _userLoginApi.LoginAsSessionUser();

        var parentPageId = parentPage.Id;
        var deletionTasks = childPages.Select(child => Task.Run(async () =>
        {
            var childId = child.Id;

            // Small delay to simulate rapid user clicks (but not simultaneous)
            await Task.Delay(Random.Shared.Next(10,
                50)); // Make actual HTTP API call - this creates a real request with proper DI scope
            var requestBody = new { pageToDeleteId = childId, parentForQuestionsId = parentPageId };
            var result =
                await _testHarness.ApiPost<DeletePageStoreController.DeleteResponse>("/apiVue/DeletePageStore/Delete",
                    requestBody);

            return result;
        })).ToList();
        var results = await Task.WhenAll(deletionTasks);

        // Assert
        await ReloadCaches();
        var newTree = TreeRenderer.ToAsciiDiagram(cachedParent!);
        await Verify(new
        {
            deleteResult = results,
            originalTree,
            newTree,
            PageVerificationData =
                await _testHarness.GetDefaultPageVerificationDataAsync() // Meilisearch wait happens automatically
        });
    }
}