using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AddParentCategoryInDb : IJob
    {
        private readonly SessionUser _sessionUser;

        public AddParentCategoryInDb(SessionUser sessionUser)
        {
            _sessionUser = sessionUser;
        }
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var childCategoryId = dataMap.GetInt("childCategoryId");
            var parentCategoryId = dataMap.GetInt("parentCategoryId");
            Logg.r().Information("Job started - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);
            Run(childCategoryId, parentCategoryId);
            Logg.r().Information("Job ended - ModifyRelation Child: {childId}, Parent: {parentId}", childCategoryId, parentCategoryId);
        }

        private void Run(int childCategoryId, int parentCategoryId)
        {
            var catRepo = Sl.CategoryRepo;

            var childCategory = catRepo.GetById(childCategoryId);
            var parentCategory = catRepo.GetById(parentCategoryId);

            ModifyRelationsForCategory.AddParentCategory(childCategory, parentCategoryId);

            catRepo.Update(childCategory, _sessionUser.User, type: CategoryChangeType.Relations);
            catRepo.Update(parentCategory, _sessionUser.User, type: CategoryChangeType.Relations);
        }
    }
}