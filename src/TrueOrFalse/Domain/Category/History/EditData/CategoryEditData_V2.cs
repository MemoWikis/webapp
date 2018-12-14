using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class CategoryEditData_V2 : CategoryEditData
{
    public IList<CategoryRelation_EditData_V2> CategoryRelations;
    public bool ImageWasUpdated;

    public CategoryEditData_V2(){}

    public CategoryEditData_V2(Category category, bool imageWasUpdated)
    {
        Name = category.Name;
        Description = category.Description;
        TopicMardkown = category.TopicMarkdown;
        WikipediaURL = category.WikipediaURL;
        DisableLearningFunctions = category.DisableLearningFunctions;
        CategoryRelations = category.CategoryRelations
            .Select(cr => new CategoryRelation_EditData_V2(cr))
            .ToList();
        ImageWasUpdated = imageWasUpdated;
    }

    public override string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static CategoryEditData_V2 CreateFromJson(string json)
    {
        return JsonConvert.DeserializeObject<CategoryEditData_V2>(json);
    }

    public override Category ToCategory(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);

        Sl.Session.Evict(category);

        category.IsHistoric = true;
        category.Name = this.Name;
        category.Description = this.Description;
        category.TopicMarkdown = this.TopicMardkown;
        category.WikipediaURL = this.WikipediaURL;
        category.DisableLearningFunctions = this.DisableLearningFunctions;

        // Historic category relations cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return category;
    }
}