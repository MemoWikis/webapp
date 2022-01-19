
using System.Collections.Generic;
using FluentNHibernate.Data;
using NHibernate.Mapping;
using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAggregatedCategoriesForQuestion : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Logg.r().Information("Job started - Update Aggregated Categories from Update Question");

            var dataMap = context.JobDetail.JobDataMap;
            var categoryIds = (List<int>)dataMap["categoryIds"];

            var aggregatedCategoriesToUpdate =
                CategoryAggregation.GetAggregatingAncestors(Sl.CategoryRepo.GetByIds(categoryIds));

            foreach (var category in aggregatedCategoriesToUpdate)
            {
                category.UpdateCountQuestionsAggregated();
                Sl.CategoryRepo.Update(category);
                KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
                Logg.r().Information("Update Category from Update Question - {id}", category.Id);
            }
        }
    }
}
