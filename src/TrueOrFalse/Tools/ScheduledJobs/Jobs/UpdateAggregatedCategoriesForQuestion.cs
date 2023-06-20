
using System.Collections.Generic;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAggregatedCategoriesForQuestion : IJob
    {
        private readonly SessionUser _sessionUser;

        public UpdateAggregatedCategoriesForQuestion(SessionUser sessionUser)
        {
            _sessionUser = sessionUser;
        }
        public void Execute(IJobExecutionContext context)
        {
            Logg.r().Information("Job started - Update Aggregated Categories from Update Question");

            var dataMap = context.JobDetail.JobDataMap;
            var categoryIds = (List<int>)dataMap["categoryIds"];

            var aggregatedCategoriesToUpdate =
                CategoryAggregation.GetAggregatingAncestors(Sl.CategoryRepo.GetByIds(categoryIds));

            foreach (var category in aggregatedCategoriesToUpdate)
            {
                category.UpdateCountQuestionsAggregated(_sessionUser.UserId);
                Sl.CategoryRepo.Update(category);
                KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
                Logg.r().Information("Update Category from Update Question - {id}", category.Id);
            }
        }
    }
}
