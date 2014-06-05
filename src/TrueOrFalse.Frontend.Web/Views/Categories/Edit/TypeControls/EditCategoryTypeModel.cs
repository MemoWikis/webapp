using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class EditCategoryTypeModel : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string WikipediaUrl { get; set; }

    public object Model;

    public const string WikipediaInfo = "Falls es einen Wikipedia-Artikel gibt, dessen Gegenstand der Kategorie entspricht, gib bitte hier den Link an.";
    public const string DescriptionInfo = "Kurze Beschreibung der Kategorie und/oder alternative Bezeichnungen.";

    

    public EditCategoryTypeModel(Category category)
    {
        if (category == null)
            return;

        

        if (category.Type == CategoryType.Book)
            Model = CategoryBook.FromJson(category.TypeJson);

        if (category.Type == CategoryType.Daily)
            Model = CategoryDaily.FromJson(category.TypeJson);

        if (category.Type == CategoryType.WebsiteVideo)
            Model = CategoryWebsiteVideo.FromJson(category.TypeJson);


        Name = category.Name;
        Description = category.Description;
        WikipediaUrl = category.WikipediaURL;
    }
}
