using Rebus.Handlers;

/// <summary>
/// Handler for updating aggregated pages when questions are modified
/// </summary>
public class UpdateAggregatedPagesHandler(
    PageRepository pageRepository,
    KnowledgeSummaryUpdateDispatcher _knowledgeSummaryUpdateDispatcher,
    ILogger logger)
    : IHandleMessages<UpdateAggregatedPagesMessage>
{
    public async Task Handle(UpdateAggregatedPagesMessage message)
    {
        try
        {
            logger.Information("Processing UpdateAggregatedPagesMessage with {PageCount} page IDs for user {UserId}",
                message.PageIds.Count, message.UserId);

            var pages = pageRepository.GetByIds(message.PageIds);
            var aggregatedPagesToUpdate = PageAggregation.GetAggregatingAncestors(pages, pageRepository);

            foreach (var page in aggregatedPagesToUpdate)
            {
                page.UpdateCountQuestionsAggregated(message.UserId);
                pageRepository.Update(page);
                _knowledgeSummaryUpdateDispatcher.SchedulePageUpdateAsync(page.Id);

                logger.Information("Updated aggregated page from question update - PageId: {PageId}", page.Id);
            }

            logger.Information("Successfully processed UpdateAggregatedPagesMessage for {PageCount} aggregated pages",
                aggregatedPagesToUpdate.Count);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error processing UpdateAggregatedPagesMessage with page IDs: {PageIds}, UserId: {UserId}",
                string.Join(",", message.PageIds), message.UserId);
            throw;
        }
    }
}
