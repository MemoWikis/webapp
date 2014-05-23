using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;


public class EditCategoryTypeModel
{
    public string WikipediaUrl;
    public object Model;

    public EditCategoryTypeModel(Category category, object model)
    {
        Model = model;

        if (category == null)
            return;

        WikipediaUrl = category.WikipediaURL;
    }
}
