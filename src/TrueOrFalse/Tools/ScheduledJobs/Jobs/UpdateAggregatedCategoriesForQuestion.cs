
using System.Collections.Generic;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAggregatedCategoriesForQuestion : IJob
    {
        private readonly SessionUser _sessionUser;
        private readonly CategoryRepository _categoryRepository;

        public UpdateAggregatedCategoriesForQuestion(SessionUser sessionUser, CategoryRepository categoryRepository)
        {
            _sessionUser = sessionUser;
            _categoryRepository = categoryRepository;
        }
        public void Execute(IJobExecutionContext context)
        {
            Logg.r().Information("Job started - Update Aggregated Categories from Update Question");

            var dataMap = context.JobDetail.JobDataMap;
            var categoryIds = (List<int>)dataMap["categoryIds"];

            var aggregatedCategoriesToUpdate =
                CategoryAggregation.GetAggregatingAncestors(_categoryRepository.GetByIds(categoryIds), _categoryRepository);

            foreach (var category in aggregatedCategoriesToUpdate)
            {
                category.UpdateCountQuestionsAggregated(_sessionUser.UserId);
                _categoryRepository.Update(category);
                KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
                Logg.r().Information("Update Category from Update Question - {id}", category.Id);
            }
        }
    }
}
