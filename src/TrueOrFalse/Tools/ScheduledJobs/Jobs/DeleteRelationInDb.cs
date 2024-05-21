using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class DeleteRelationInDb : IJob
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryRelationRepo _categoryRelationRepo;

        public DeleteRelationInDb(
            CategoryRepository categoryRepository,
            CategoryRelationRepo categoryRelationRepo)
        {
            _categoryRepository = categoryRepository;
            _categoryRelationRepo = categoryRelationRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var relationId = dataMap.GetInt("relationId");
            var authorId = dataMap.GetInt("authorId");

            await Run(relationId, authorId);
            Logg.r.Information("Job ended - DeleteRelation");
        }

        private Task Run(int relationId, int authorId)
        {
            var relationToDelete =
                relationId > 0 ? _categoryRelationRepo.GetById(relationId) : null;
            Logg.r.Information(
                "Job started - DeleteRelation RelationId: {relationId}, Child: {childId}, Parent: {parentId}",
                relationToDelete.Id, relationToDelete.Child.Id, relationToDelete.Parent.Id);

            _categoryRepository.Update(relationToDelete.Child, authorId,
                type: CategoryChangeType.Relations);
            _categoryRepository.Update(relationToDelete.Parent, authorId,
                type: CategoryChangeType.Relations);

            if (relationToDelete != null)
                _categoryRelationRepo.Delete(relationToDelete);

            return Task.CompletedTask;
        }
    }
}