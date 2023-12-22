using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAggregatedCategoriesForQuestion : IJob
    {
        private readonly JobQueueRepo _jobQueueRepo;
        private readonly CategoryRepository _categoryRepository;

        public UpdateAggregatedCategoriesForQuestion(JobQueueRepo jobQueueRepo, CategoryRepository categoryRepository)
        {
            _jobQueueRepo = jobQueueRepo;
            _categoryRepository = categoryRepository;
        }
        public Task Execute(IJobExecutionContext context)
        {
            Logg.r.Information("Job started - Update Aggregated Categories from Update Question");

            var dataMap = context.JobDetail.JobDataMap;
            var categoryIds = (List<int>)dataMap["categoryIds"];
            var userId = (int)dataMap["userId"];

            var aggregatedCategoriesToUpdate =
                CategoryAggregation.GetAggregatingAncestors(_categoryRepository.GetByIds(categoryIds), _categoryRepository);

            foreach (var category in aggregatedCategoriesToUpdate)
            {
                category.UpdateCountQuestionsAggregated(userId);
                _categoryRepository.Update(category);
                KnowledgeSummaryUpdate.ScheduleForCategory(category.Id, _jobQueueRepo);
                Logg.r.Information("Update Category from Update Question - {id}", category.Id);
            }

            return Task.CompletedTask;
        }
    }
}
