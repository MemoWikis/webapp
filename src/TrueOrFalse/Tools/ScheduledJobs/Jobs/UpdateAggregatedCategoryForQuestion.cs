
using FluentNHibernate.Data;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAggregatedCategoryForQuestion : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Logg.r().Information("Job started - Update Category from Update Question");

            var dataMap = context.JobDetail.JobDataMap;
            var category = Sl.CategoryRepo.GetById(dataMap.GetInt("categoryId"));

            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
            Logg.r().Information("Update Category from Update Question - {id}", category.Id);
        }
    }
}
