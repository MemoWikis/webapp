using Newtonsoft.Json;
using NHibernate;

public class CategoryEditData_V1 : CategoryEditData
{
    private readonly ISession _nhibernateSession;
    private readonly PageRepository _pageRepository;
    public IList<PageRelationEditDataV1> CategoryRelations;

    public CategoryEditData_V1(
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

    public CategoryEditData_V1()
    {
    }

    public override Page ToCategory(int categoryId)
    {
        var category = _pageRepository.GetById(categoryId);

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

    public static CategoryEditData_V1 CreateFromJson(string json) =>
        JsonConvert.DeserializeObject<CategoryEditData_V1>(json);

    //placeholder
    public override PageCacheItem ToCacheCategory(int categoryId) => new PageCacheItem();
}