using Quartz;

public class UpdateAggregatedPagesForQuestion : IJob
{
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly PageRepository _pageRepository;

    public UpdateAggregatedPagesForQuestion(JobQueueRepo jobQueueRepo, PageRepository pageRepository)
    {
        _jobQueueRepo = jobQueueRepo;
        _pageRepository = pageRepository;
    }

    public Task Execute(IJobExecutionContext context)
    {
        Logg.r.Information("Job started - Update Aggregated Pages from Update Question");

        var dataMap = context.JobDetail.JobDataMap;
        var pageIds = (List<int>)dataMap["pageIds"];
        var userId = (int)dataMap["userId"];

        var aggregatedPagesToUpdate =
            PageAggregation.GetAggregatingAncestors(_pageRepository.GetByIds(pageIds), _pageRepository);

        foreach (var pages in aggregatedPagesToUpdate)
        {
            pages.UpdateCountQuestionsAggregated(userId);
            _pageRepository.Update(pages);
            KnowledgeSummaryUpdate.ScheduleForPage(pages.Id, _jobQueueRepo);
            Logg.r.Information("Update Page from Update Question - {id}", pages.Id);
        }

        return Task.CompletedTask;
    }
}