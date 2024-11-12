using Newtonsoft.Json;
using NHibernate;

public class PageEditData_V2 : PageEditData
{
    private readonly PageRepository _pageRepository;
    public IList<PageRelation_EditData_V2> PageRelations;
    public bool ImageWasUpdated;
    private readonly ISession _nhibernateSession;

    //empty constructor is used for the JsonSerializer
    public PageEditData_V2()
    {
    }

    public PageEditData_V2(PageRepository pageRepository)
    {
        _pageRepository = pageRepository;
    }

    public PageEditData_V2(
        Page page,
        bool imageWasUpdated,
        ISession nhibernateSession,
        int[]? parentIds,
        int[]? childIds,
        int? deleteChangeId = null,
        string? deletedName = null,
        PageVisibility? deletedVisibility = null)
    {
        Name = page.Name;
        Description = page.Description;
        PageMardkown = page.Markdown;
        Content = page.Content;
        PageRelations = null;
        ImageWasUpdated = imageWasUpdated;
        _nhibernateSession = nhibernateSession;
        Visibility = page.Visibility;
        ParentIds = parentIds;
        ChildIds = childIds;
        DeleteChangeId = deleteChangeId;
        DeletedName = deletedName;

        if (deleteChangeId != null && deletedVisibility != null && deleteChangeId > 0)
            Visibility = deletedVisibility.Value;
    }

    public override string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static PageEditData_V2 CreateFromJson(string json)
    {
        json = json == null ? "" : json;
        return JsonConvert.DeserializeObject<PageEditData_V2>(json);
    }

    public override Page ToPage(int pageId)
    {
        var page = _pageRepository.GetById(pageId);
        _nhibernateSession.Evict(page);

        page = page == null ? new Page() : page;
        page.IsHistoric = true;
        page.Name = this.Name;
        page.Description = this.Description;
        page.Markdown = this.PageMardkown;
        page.Content = this.Content;
        page.Visibility = this.Visibility;

        // Historic page relations cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return page;
    }

    public override PageCacheItem ToCachePage(int pageId)
    {
        var page = EntityCache.GetPage(pageId);
        //_nhibernateSession.Evict(page);

        page = page == null ? new PageCacheItem() : page;
        page.IsHistoric = true;
        page.Name = this.Name;
        page.Description = this.Description;
        page.PageMarkdown = this.PageMardkown;
        page.Content = this.Content;
        page.CustomSegments = this.CustomSegments;
        page.WikipediaURL = this.WikipediaURL;
        page.DisableLearningFunctions = this.DisableLearningFunctions;
        page.Visibility = this.Visibility;

        // Historic page relations cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return page;
    }
}