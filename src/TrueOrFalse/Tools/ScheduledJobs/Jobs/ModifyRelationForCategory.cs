using Quartz;

namespace TrueOrFalse.Tools.ScheduledJobs.Jobs
{
    class ModifyRelationForCategory : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                var parentCategory = EntityCache.GetCategory(CategoryData.ParentCategoryId);
                ModifyRelationsForCategory.AddParentCategory(CategoryData.Category, parentCategory);
                Sl.CategoryRepo.Create(CategoryData.Category);

            }, "ModifyRelationForCategoryJob");
        }
    }

    public class CategoryData
    {
        public static int  ParentCategoryId { get; set; }
        public static Category Category { get; set; }
        public static CategoryType Type { get; set; }

    }
}


