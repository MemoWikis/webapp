using Quartz;
using System.Text.Json;

public class AddOrUpdateRelationsInDb : IJob
{
    private readonly PageRepository _pageRepository;
    private readonly PageRelationRepo _pageRelationRepo;
    private int _authorId;

    public AddOrUpdateRelationsInDb(
        PageRepository pageRepository,
        PageRelationRepo pageRelationRepo)
    {
        _pageRepository = pageRepository;
        _pageRelationRepo = pageRelationRepo;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var relationsJson = dataMap.GetString("relations");
        var relations = JsonSerializer.Deserialize<List<PageRelationCache>>(relationsJson);
        _authorId = dataMap.GetInt("authorId");

        await Run(relations);
        Logg.r.Information("Job ended - ModifyRelations");
    }

    private Task Run(List<PageRelationCache> relations)
    {
        foreach (var r in relations)
        {
            Logg.r.Information(
                "Job started - ModifyRelations RelationId: {relationId}, Child: {childId}, Parent: {parentId}",
                r.Id, r.ChildId, r.ParentId);

            var relationToUpdate = r.Id > 0 ? _pageRelationRepo.GetById(r.Id) : null;
            var child = _pageRepository.GetById(r.ChildId);
            var parent = _pageRepository.GetById(r.ParentId);

            if (relationToUpdate != null)
            {
                relationToUpdate.Child = child;
                relationToUpdate.Parent = parent;
                relationToUpdate.PreviousId = r.PreviousId;
                relationToUpdate.NextId = r.NextId;

                _pageRelationRepo.Update(relationToUpdate);
            }
            else
            {
                var relation = new PageRelation
                {
                    Child = child,
                    Parent = parent,
                    PreviousId = r.PreviousId,
                    NextId = r.NextId,
                };

                _pageRelationRepo.Create(relation);
            }

            _pageRepository.Update(child, _authorId, type: PageChangeType.Relations);
            _pageRepository.Update(parent, _authorId, type: PageChangeType.Relations);
        }

        return Task.CompletedTask;
    }
}