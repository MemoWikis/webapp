using Rebus.Handlers;

/// <summary>
/// Handler for updating aggregated pages when questions are modified
/// </summary>
public class UpdateAggregatedPagesHandler : IHandleMessages<UpdateAggregatedPagesMessage>
{
    private readonly PageRepository _pageRepository;
    private readonly KnowledgeSummaryUpdateService _knowledgeSummaryUpdateService;
    private readonly ILogger _logger;

    public UpdateAggregatedPagesHandler(
        PageRepository pageRepository,
        KnowledgeSummaryUpdateService knowledgeSummaryUpdateService,
        ILogger logger)
    {
        _pageRepository = pageRepository;
        _knowledgeSummaryUpdateService = knowledgeSummaryUpdateService;
        _logger = logger;
    }

    public async Task Handle(UpdateAggregatedPagesMessage message)
    {
        try
        {
            _logger.Information("Processing UpdateAggregatedPagesMessage with {PageCount} page IDs for user {UserId}", 
                message.PageIds.Count, message.UserId);

            var pages = _pageRepository.GetByIds(message.PageIds);
            var aggregatedPagesToUpdate = PageAggregation.GetAggregatingAncestors(pages, _pageRepository);

            foreach (var page in aggregatedPagesToUpdate)
            {
                page.UpdateCountQuestionsAggregated(message.UserId);
                _pageRepository.Update(page);
                _knowledgeSummaryUpdateService.SchedulePageUpdate(page.Id);

                _logger.Information("Updated aggregated page from question update - PageId: {PageId}", page.Id);
            }

            _logger.Information("Successfully processed UpdateAggregatedPagesMessage for {PageCount} aggregated pages", 
                aggregatedPagesToUpdate.Count);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing UpdateAggregatedPagesMessage with page IDs: {PageIds}, UserId: {UserId}", 
                string.Join(",", message.PageIds), message.UserId);
            throw;
        }
    }
}
