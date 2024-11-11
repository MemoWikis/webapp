using Newtonsoft.Json;
using NHibernate;

public class PageEditDataV1 : PageEditData
{
    private readonly ISession _nhibernateSession;
    private readonly PageRepository _pageRepository;
    public IList<PageRelationEditDataV1> CategoryRelations;

    public PageEditDataV1(
        Page page,
        ISession nhibernateSession,
        PageRepository pageRepository)
    {
        _nhibernateSession = nhibernateSession;
        _pageRepository = pageRepository;
        Name = page.Name;
        Description = page.Description;
        TopicMardkown = page.TopicMarkdown;
        Content = page.Content;
        CustomSegments = page.CustomSegments;
        WikipediaURL = page.WikipediaURL;
        DisableLearningFunctions = page.DisableLearningFunctions;
        CategoryRelations = null;
        Visibility = page.Visibility;
    }

    public PageEditDataV1()
    {
    }

    public override Page ToPage(int pageId)
    {
        var category = _pageRepository.GetById(pageId);

        _nhibernateSession.Evict(category);
        var categoryIsNull = category == null;
        category = categoryIsNull ? new Page() : category;

        category.IsHistoric = true;
        category.Name = this.Name;
        category.Description = this.Description;
        category.TopicMarkdown = this.TopicMardkown;
        category.CustomSegments = this.CustomSegments;
        category.Content = this.Content;
        category.WikipediaURL = this.WikipediaURL;
        category.DisableLearningFunctions = this.DisableLearningFunctions;
        category.Visibility = this.Visibility;

        // Historic CategoryRelations cannot be loaded for DataVersion 1 because there
        // was a bug where data didn't get written properly so correct relation data
        // simply do not exist for V1.
        // Also they cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return category;
    }

    public override string ToJson() => JsonConvert.SerializeObject(this);

    public static PageEditDataV1 CreateFromJson(string json) =>
        JsonConvert.DeserializeObject<PageEditDataV1>(json);

    //placeholder
    public override PageCacheItem ToCachePage(int pageId) => new PageCacheItem();
}