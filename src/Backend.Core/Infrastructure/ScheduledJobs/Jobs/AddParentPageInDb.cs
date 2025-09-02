using Quartz;

public class AddParentPageInDb : IJob
{
    private readonly PageRepository _pageRepository;
    private readonly PageRelationRepo _pageRelationRepo;
    private readonly KnowledgeSummaryUpdateDispatcher _knowledgeSummaryUpdateDispatcher;
    private int _authorId;

    public AddParentPageInDb(PageRepository pageRepository, PageRelationRepo pageRelationRepo, KnowledgeSummaryUpdateDispatcher _knowledgeSummaryUpdateDispatcher)
    {
        _pageRepository = pageRepository;
        _pageRelationRepo = pageRelationRepo;
        this._knowledgeSummaryUpdateDispatcher = _knowledgeSummaryUpdateDispatcher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //context.JobDetail.JobDataMap["context"];
        var dataMap = context.JobDetail.JobDataMap;
        var childPageId = dataMap.GetInt("childPageId");
        var parentPageId = dataMap.GetInt("parentPageId");
        _authorId = dataMap.GetInt("authorId");

        Log.Information("Job started - ModifyRelation Child: {childId}, Parent: {parentId}", childPageId, parentPageId);
        await Run(childPageId, parentPageId);
        Log.Information("Job ended - ModifyRelation Child: {childId}, Parent: {parentId}", childPageId, parentPageId);
    }

    private Task Run(int childPageId, int parentPageId)
    {
        var childPage = _pageRepository.GetById(childPageId);
        var parentPage = _pageRepository.GetById(parentPageId);

        new ModifyRelationsForPage(_pageRepository, _pageRelationRepo, _knowledgeSummaryUpdateDispatcher).AddParentPage(childPage, parentPageId);

        _pageRepository.Update(childPage, _authorId, type: PageChangeType.Relations);
        _pageRepository.Update(parentPage, _authorId, type: PageChangeType.Relations);
        return Task.CompletedTask;
    }
}