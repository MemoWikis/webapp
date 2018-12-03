﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class CategoryEditData_V1 : CategoryEditData
{
    public IList<CategoryRelation_EditData_V1> CategoryRelations;

    public CategoryEditData_V1(){}

    public CategoryEditData_V1(Category category)
    {
        Name = category.Name;
        Description = category.Description;
        TopicMardkown = category.TopicMarkdown;
        WikipediaURL = category.WikipediaURL;
        DisableLearningFunctions = category.DisableLearningFunctions;
        CategoryRelations = category.CategoryRelations
            .Select(cr => new CategoryRelation_EditData_V1(cr))
            .ToList();
    }

    public override string ToJson() => JsonConvert.SerializeObject(this);

    public static CategoryEditData_V1 CreateFromJson(string json) => JsonConvert.DeserializeObject<CategoryEditData_V1>(json);

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

        //we decided not to load historic CategoryRelations
        //because it seems to complicated

        return category;
    }
}