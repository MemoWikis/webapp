using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;


public class EditCategoryTypeModel
{
    public string WikipediaUrl;
    public object Model;

    public EditCategoryTypeModel(Category category)
    {
        if (category == null)
            return;

        if (category.Type == CategoryType.WebsiteVideo)
            Model = CategoryWebsiteVideo.FromJson(category.TypeJson);

        if (category.Type == CategoryType.Book)
            Model = CategoryBook.FromJson(category.TypeJson);

        WikipediaUrl = category.WikipediaURL;
    }
}
