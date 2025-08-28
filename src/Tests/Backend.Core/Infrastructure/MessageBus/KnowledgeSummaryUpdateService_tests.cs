using NUnit.Framework;

[TestFixture]
internal class KnowledgeSummaryUpdateService_tests : BaseTestHarness
{
    [Test]
    public void Should_Send_Debounced_Page_Update_Message_Sync()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var pageId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrow(() => 
            knowledgeSummaryUpdateService.SchedulePageUpdate(pageId));
    }

    [Test]
    public async Task Should_Send_Debounced_Page_Update_Message_Async()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var pageId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrowAsync(async () => 
            await knowledgeSummaryUpdateService.SchedulePageUpdateAsync(pageId));
    }

    [Test]
    public void Should_Send_Debounced_User_Update_Message_Sync()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var userId = 456;
        var pageId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrow(() => 
            knowledgeSummaryUpdateService.ScheduleUserUpdate(userId, pageId));
    }

    [Test]
    public async Task Should_Send_Debounced_User_Update_Message_Async()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var userId = 456;
        var pageId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrowAsync(async () => 
            await knowledgeSummaryUpdateService.ScheduleUserUpdateAsync(userId, pageId));
    }

    [Test]
    public void Should_Send_Multiple_Page_Updates_Sync()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var pageIds = new[] { 123, 456, 789 };

        // Act & Assert - Should not throw
        Assert.DoesNotThrow(() => 
            knowledgeSummaryUpdateService.SchedulePageUpdates(pageIds));
    }

    [Test]
    public async Task Should_Send_Multiple_Page_Updates_Async()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var pageIds = new[] { 123, 456, 789 };

        // Act & Assert - Should not throw
        Assert.DoesNotThrowAsync(async () => 
            await knowledgeSummaryUpdateService.SchedulePageUpdatesAsync(pageIds));
    }

    [Test]
    public void Should_Send_Debounced_UserAndPage_Update_Message_Sync()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var userId = 456;
        var pageId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrow(() => 
            knowledgeSummaryUpdateService.ScheduleUserAndPageUpdate(userId, pageId));
    }

    [Test]
    public async Task Should_Send_Debounced_UserAndPage_Update_Message_Async()
    {
        // Arrange
        var knowledgeSummaryUpdateService = R<KnowledgeSummaryUpdateService>();
        var userId = 456;
        var pageId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrowAsync(async () => 
            await knowledgeSummaryUpdateService.ScheduleUserAndPageUpdateAsync(userId, pageId));
    }
}
