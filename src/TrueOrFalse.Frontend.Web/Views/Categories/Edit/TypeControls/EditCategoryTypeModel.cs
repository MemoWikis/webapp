using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;


public class EditCategoryTypeModel : BaseModel
{
    public string Name;
    public string Description;
    public string WikipediaUrl;
    public object Model;

    public const string DescriptionHelp = "test";

    public EditCategoryTypeModel(Category category)
    {
        if (category == null)
            return;

        if (category.Type == CategoryType.WebsiteVideo)
            Model = CategoryWebsiteVideo.FromJson(category.TypeJson);

        if (category.Type == CategoryType.Book)
            Model = CategoryBook.FromJson(category.TypeJson);

        Name = category.Name;
        Description = category.Description;
        WikipediaUrl = category.WikipediaURL;
    }
}
