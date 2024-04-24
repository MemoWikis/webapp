using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class AddParentCategoryInDb : IJob
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryRelationRepo _categoryRelationRepo;
        private int _authorId;

        public AddParentCategoryInDb(
            CategoryRepository categoryRepository,
            CategoryRelationRepo categoryRelationRepo)
        {
            _categoryRepository = categoryRepository;
            _categoryRelationRepo = categoryRelationRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //context.JobDetail.JobDataMap["context"];
            var dataMap = context.JobDetail.JobDataMap;
            var childCategoryId = dataMap.GetInt("childCategoryId");
            var parentCategoryId = dataMap.GetInt("parentCategoryId");
            _authorId = dataMap.GetInt("authorId");

            Logg.r.Information("Job started - ModifyRelation Child: {childId}, Parent: {parentId}",
                childCategoryId, parentCategoryId);
            await Run(childCategoryId, parentCategoryId);
            Logg.r.Information("Job ended - ModifyRelation Child: {childId}, Parent: {parentId}",
                childCategoryId, parentCategoryId);
        }

        private Task Run(int childCategoryId, int parentCategoryId)
        {
            var childCategory = _categoryRepository.GetById(childCategoryId);
            var parentCategory = _categoryRepository.GetById(parentCategoryId);

            new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo)
                .AddParentAsync(childCategory, parentCategoryId);

            _categoryRepository.Update(childCategory, _authorId,
                type: CategoryChangeType.Relations);
            _categoryRepository.Update(parentCategory, _authorId,
                type: CategoryChangeType.Relations);
            return Task.CompletedTask;
        }
    }
}