using Quartz;

public class UpdateAggregatedPagesForQuestion(PageRepository _pageRepository, KnowledgeSummaryUpdateDispatcher _knowledgeSummaryUpdateDispatcher) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Log.Information("Job started - Update Aggregated Pages from Update Question");

        var dataMap = context.JobDetail.JobDataMap;
        var pageIds = (List<int>)dataMap["pageIds"];
        var userId = (int)dataMap["userId"];

        var aggregatedPagesToUpdate =
            PageAggregation.GetAggregatingAncestors(_pageRepository.GetByIds(pageIds), _pageRepository);

        foreach (var page in aggregatedPagesToUpdate)
        {
            page.UpdateCountQuestionsAggregated(userId);
            _pageRepository.Update(page);
            _knowledgeSummaryUpdateDispatcher.SchedulePageUpdateAsync(page.Id);

            Log.Information("Update Page from Update Question - {id}", page.Id);
        }

        return Task.CompletedTask;
    }
}