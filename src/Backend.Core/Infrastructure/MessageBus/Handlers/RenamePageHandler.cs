using Rebus.Handlers;

/// <summary>
/// Handler for renaming pages
/// </summary>
public class RenamePageHandler : IHandleMessages<RenamePageMessage>
{
    private readonly ILogger _logger;
    private readonly PageRepository _pageRepository;

    public RenamePageHandler(ILogger logger, PageRepository pageRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _pageRepository = pageRepository ?? throw new ArgumentNullException(nameof(pageRepository));
    }

    public async Task Handle(RenamePageMessage message)
    {
        _logger.Information("Processing rename page message for PageId: {PageId} to name: {NewName}",
            message.PageId, message.NewName);

        var page = EntityCache.GetPage(message.PageId);
        if (page != null)
        {
            page.Name = message.NewName;
            EntityCache.AddOrUpdate(page);

            _logger.Information("Successfully renamed page {PageId} to {NewName}",
                message.PageId, message.NewName);
        }
        else
        {
            _logger.Warning("Page with ID {PageId} not found for renaming", message.PageId);
        }

        await Task.CompletedTask;
    }
}
