using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHibernate;

public class CategoryEditData_V1 : CategoryEditData
{
    private readonly ISession _nhibernateSession;
    private readonly CategoryRepository _categoryRepository;
    public IList<CategoryRelation_EditData_V1> CategoryRelations;

    public CategoryEditData_V1(Category category,
        ISession nhibernateSession,
        CategoryRepository categoryRepository)
    {
        _nhibernateSession = nhibernateSession;
        _categoryRepository = categoryRepository;
        Name = category.Name;
        Description = category.Description;
        TopicMardkown = category.TopicMarkdown;
        Content = category.Content;
        CustomSegments = category.CustomSegments;
        WikipediaURL = category.WikipediaURL;
        DisableLearningFunctions = category.DisableLearningFunctions;
        CategoryRelations = category.CategoryRelations
            .Select(cr => new CategoryRelation_EditData_V1(cr))
            .ToList();
        Visibility = category.Visibility;
    }

    public override Category ToCategory(int categoryId)
    {
        var category = _categoryRepository.GetById(categoryId);

        _nhibernateSession.Evict(category);
        var categoryIsNull = category == null;
        category = categoryIsNull ? new Category() : category;

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

    public static CategoryEditData_V1 CreateFromJson(string json) => JsonConvert.DeserializeObject<CategoryEditData_V1>(json);

  
}