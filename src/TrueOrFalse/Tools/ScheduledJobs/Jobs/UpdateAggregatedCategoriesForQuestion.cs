using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAggregatedCategoriesForQuestion : IJob
    {
        private readonly JobQueueRepo _jobQueueRepo;
        private readonly PageRepository _pageRepository;

        public UpdateAggregatedCategoriesForQuestion(JobQueueRepo jobQueueRepo, PageRepository pageRepository)
        {
            _jobQueueRepo = jobQueueRepo;
            _pageRepository = pageRepository;
        }
        public Task Execute(IJobExecutionContext context)
        {
            Logg.r.Information("Job started - Update Aggregated Categories from Update Question");

            var dataMap = context.JobDetail.JobDataMap;
            var categoryIds = (List<int>)dataMap["categoryIds"];
            var userId = (int)dataMap["userId"];

            var aggregatedCategoriesToUpdate =
                CategoryAggregation.GetAggregatingAncestors(_pageRepository.GetByIds(categoryIds), _pageRepository);

            foreach (var category in aggregatedCategoriesToUpdate)
            {
                category.UpdateCountQuestionsAggregated(userId);
                _pageRepository.Update(category);
                KnowledgeSummaryUpdate.ScheduleForPage(category.Id, _jobQueueRepo);
                Logg.r.Information("Update Category from Update Question - {id}", category.Id);
            }

            return Task.CompletedTask;
        }
    }
}
