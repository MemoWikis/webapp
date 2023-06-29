using Quartz;


namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AddParentCategoryInDb : IJob
    {
        private readonly SessionUser _sessionUser;
        private readonly CategoryRepository _categoryRepository;

        public AddParentCategoryInDb(SessionUser sessionUser, CategoryRepository categoryRepository)
        {
            _sessionUser = sessionUser;
            _categoryRepository = categoryRepository;
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
            var childCategory = _categoryRepository.GetById(childCategoryId);
            var parentCategory = _categoryRepository.GetById(parentCategoryId);

            new ModifyRelationsForCategory(_categoryRepository).AddParentCategory(childCategory, parentCategoryId);

            _categoryRepository.Update(childCategory, _sessionUser.User, type: CategoryChangeType.Relations);
            _categoryRepository.Update(parentCategory, _sessionUser.User, type: CategoryChangeType.Relations);
        }
    }
}