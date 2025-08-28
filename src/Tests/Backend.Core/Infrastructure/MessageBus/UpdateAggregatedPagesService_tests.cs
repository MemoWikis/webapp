using NUnit.Framework;

[TestFixture]
internal class UpdateAggregatedPagesService_tests : BaseTestHarness
{
    [Test]
    public void Should_Send_UpdateAggregatedPages_Message_Sync()
    {
        // Arrange
        var updateAggregatedPagesService = R<UpdateAggregatedPagesService>();
        var pageIds = new List<int> { 123, 456, 789 };
        var userId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrow(() => 
            updateAggregatedPagesService.UpdateAggregatedPages(pageIds, userId));
    }

    [Test]
    public async Task Should_Send_UpdateAggregatedPages_Message_Async()
    {
        // Arrange
        var updateAggregatedPagesService = R<UpdateAggregatedPagesService>();
        var pageIds = new List<int> { 123, 456, 789 };
        var userId = 123;

        // Act & Assert - Should not throw
        Assert.DoesNotThrowAsync(async () => 
            await updateAggregatedPagesService.UpdateAggregatedPagesAsync(pageIds, userId));
    }
}
