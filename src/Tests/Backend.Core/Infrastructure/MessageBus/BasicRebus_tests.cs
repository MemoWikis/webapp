[TestFixture]
internal class BasicRebus_tests : BaseTestHarness
{
    [Test]
    public void MessageBusService_Should_Be_Registered()
    {
        // Arrange & Act
        var messageBusService = R<IMessageBusService>();

        // Assert
        Assert.That(messageBusService, Is.Not.Null);
        Assert.That(messageBusService, Is.TypeOf<RebusMessageBusService>());
    }

    [Test]
    public void Should_Send_Test_Message()
    {
        // Arrange
        var messageBusService = R<IMessageBusService>();
        var testMessage = new TestMessage("Hello from Rebus!");

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await messageBusService.SendAsync(testMessage));
    }

    [Test]
    public async Task Should_Rename_Page_Via_Rebus()
    {
        // Arrange
        await ClearData();

        var context = NewPageContext();
        var user = new User { Id = 1 };

        context.Add("Before RebusNameChange", creator: user, isWiki: true).Persist();

        var page = context.GetPageByName("Before RebusNameChange");

        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var originalCachedPage = EntityCache.GetPage(page);
        var originalName = originalCachedPage?.Name;

        var messageBusService = R<IMessageBusService>();
        var renameMessage = new RenamePageMessage(page.Id, "After RebusNameChange");

        // Act
        await messageBusService.SendAsync(renameMessage);

        // Wait for message processing
        await Task.Delay(500);

        var updatedCachedPage = EntityCache.GetPage(page);

        // Assert
        await Verify(new
        {
            originalName,
            updatedName = updatedCachedPage?.Name,
        });
    }
}
