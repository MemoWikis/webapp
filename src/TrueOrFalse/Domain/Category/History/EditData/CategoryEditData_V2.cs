using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class CategoryEditData_V2 : CategoryEditData
{
    public IList<CategoryRelation_EditData_V2> CategoryRelations;

    public CategoryEditData_V2(){}

    public CategoryEditData_V2(Category category)
    {
        Name = category.Name;
        Description = category.Description;
        TopicMardkown = category.TopicMarkdown;
        WikipediaURL = category.WikipediaURL;
        DisableLearningFunctions = category.DisableLearningFunctions;
        CategoryRelations = category.CategoryRelations
            .Select(cr => new CategoryRelation_EditData_V2(cr))
            .ToList();
}

    public override string ToJson() => JsonConvert.SerializeObject(this);

    public static CategoryEditData_V2 CreateFromJson(string json) => JsonConvert.DeserializeObject<CategoryEditData_V2>(json);
}