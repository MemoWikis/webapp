using Newtonsoft.Json;
using NHibernate;

public class CategoryEditData_V2 : CategoryEditData
{
    private readonly CategoryRepository _categoryRepository;
    public IList<CategoryRelation_EditData_V2> CategoryRelations;
    public bool ImageWasUpdated;
    private readonly ISession _nhibernateSession;

    //empty constructor is used for the JsonSerializer
    public CategoryEditData_V2()
    {
    }

    public CategoryEditData_V2(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public CategoryEditData_V2(
        Category category,
        bool imageWasUpdated,
        int[] affectedParentIdsByMove,
        ISession nhibernateSession)
    {
        Name = category.Name;
        Description = category.Description;
        TopicMardkown = category.TopicMarkdown;
        Content = category.Content;
        CategoryRelations = null;
        ImageWasUpdated = imageWasUpdated;
        _nhibernateSession = nhibernateSession;
        Visibility = category.Visibility;
        AffectedParentIds = affectedParentIdsByMove ?? new int[] { };
    }

    public override string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static CategoryEditData_V2 CreateFromJson(string json)
    {
        json = json == null ? "" : json;
        return JsonConvert.DeserializeObject<CategoryEditData_V2>(json);
    }

    public override Category ToCategory(int categoryId)
    {
        var category = _categoryRepository.GetById(categoryId);
        _nhibernateSession.Evict(category);

        category = category == null ? new Category() : category;
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

    public override CategoryCacheItem ToCacheCategory(int categoryId)
    {
        var category = EntityCache.GetCategory(categoryId);
        //_nhibernateSession.Evict(category);

        category = category == null ? new CategoryCacheItem() : category;
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