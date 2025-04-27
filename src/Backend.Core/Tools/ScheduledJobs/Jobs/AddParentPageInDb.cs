using Quartz;

public class AddParentPageInDb : IJob
{
    private readonly PageRepository _pageRepository;
    private readonly PageRelationRepo _pageRelationRepo;
    private int _authorId;

    public AddParentPageInDb(PageRepository pageRepository, PageRelationRepo pageRelationRepo)
    {
        _pageRepository = pageRepository;
        _pageRelationRepo = pageRelationRepo;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //context.JobDetail.JobDataMap["context"];
        var dataMap = context.JobDetail.JobDataMap;
        var childPageId = dataMap.GetInt("childPageId");
        var parentPageId = dataMap.GetInt("parentPageId");
        _authorId = dataMap.GetInt("authorId");

        Logg.r.Information("Job started - ModifyRelation Child: {childId}, Parent: {parentId}", childPageId, parentPageId);
        await Run(childPageId, parentPageId);
        Logg.r.Information("Job ended - ModifyRelation Child: {childId}, Parent: {parentId}", childPageId, parentPageId);
    }

    private Task Run(int childPageId, int parentPageId)
    {
        var childPage = _pageRepository.GetById(childPageId);
        var parentPage = _pageRepository.GetById(parentPageId);

        new ModifyRelationsForPage(_pageRepository, _pageRelationRepo).AddParentPage(childPage, parentPageId);

        _pageRepository.Update(childPage, _authorId, type: PageChangeType.Relations);
        _pageRepository.Update(parentPage, _authorId, type: PageChangeType.Relations);
        return Task.CompletedTask;
    }
}