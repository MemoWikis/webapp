using Quartz;

public class DeleteRelationInDb : IJob
{
    private readonly PageRepository _pageRepository;
    private readonly PageRelationRepo _pageRelationRepo;

    public DeleteRelationInDb(
        PageRepository pageRepository,
        PageRelationRepo pageRelationRepo)
    {
        _pageRepository = pageRepository;
        _pageRelationRepo = pageRelationRepo;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var relationId = dataMap.GetInt("relationId");
        var authorId = dataMap.GetInt("authorId");

        await Run(relationId, authorId);
        Log.Information("Job ended - DeleteRelation");
    }

    private Task Run(int relationId, int authorId)
    {
        var relationToDelete =
            relationId > 0 ? _pageRelationRepo.GetById(relationId) : null;
        Log.Information(
            "Job started - DeleteRelation RelationId: {relationId}, Child: {childId}, Parent: {parentId}",
            relationToDelete.Id, relationToDelete.Child.Id, relationToDelete.Parent.Id);

        _pageRepository.Update(relationToDelete.Child, authorId,
            type: PageChangeType.Relations);
        _pageRepository.Update(relationToDelete.Parent, authorId,
            type: PageChangeType.Relations);

        if (relationToDelete != null)
            _pageRelationRepo.Delete(relationToDelete);

        return Task.CompletedTask;
    }
}