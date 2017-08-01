using System.Collections.Generic;
using System.Linq;

public class CategoryEditData_V1
{
    public string Name;
    public string Description;
    public string WikipediaURL;
    public bool DisableLearningFunctions;

    public IList<CategoryRelation_EditData_V1> CategoryRelations;

    public virtual string CategoriesToExcludeIdsString { get; set; }
    public virtual string CategoriesToIncludeIdsString { get; set; }

    public string FeaturedSetsIdsString;

    public CategoryEditData_V1(Category category)
    {
        Name = category.Name;
        Description = category.Description;
        WikipediaURL = category.WikipediaURL;
        DisableLearningFunctions = category.DisableLearningFunctions;

        CategoryRelations = category.CategoryRelations.Select(cr => new CategoryRelation_EditData_V1(cr)).ToList();
    }
}