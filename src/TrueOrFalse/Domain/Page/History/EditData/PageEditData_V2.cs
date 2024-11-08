using Newtonsoft.Json;
using NHibernate;

public class PageEditData_V2 : CategoryEditData
{
    private readonly PageRepository _pageRepository;
    public IList<PageRelation_EditData_V2> CategoryRelations;
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
        TopicMardkown = page.TopicMarkdown;
        Content = page.Content;
        CategoryRelations = null;
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

    public override Page ToCategory(int categoryId)
    {
        var category = _pageRepository.GetById(categoryId);
        _nhibernateSession.Evict(category);

        category = category == null ? new Page() : category;
        category.IsHistoric = true;
        category.Name = this.Name;
        category.Description = this.Description;
        category.TopicMarkdown = this.TopicMardkown;
        category.Content = this.Content;
        category.Visibility = this.Visibility;

        // Historic category relations cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return category;
    }

    public override PageCacheItem ToCacheCategory(int categoryId)
    {
        var category = EntityCache.GetPage(categoryId);
        //_nhibernateSession.Evict(category);

        category = category == null ? new PageCacheItem() : category;
        category.IsHistoric = true;
        category.Name = this.Name;
        category.Description = this.Description;
        category.TopicMarkdown = this.TopicMardkown;
        category.Content = this.Content;
        category.CustomSegments = this.CustomSegments;
        category.WikipediaURL = this.WikipediaURL;
        category.DisableLearningFunctions = this.DisableLearningFunctions;
        category.Visibility = this.Visibility;

        // Historic category relations cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return category;
    }
}