using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Web;

public class EditCategoryModel : BaseModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public UIMessage Message;

    public List<string> ParentCategories = new List<string>();

    public bool IsEditing { get; set; }

    public string ImageUrl { get; set; }

    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }
    public string ImageGuid { get; set; }
    public string ImageLicenceOwner { get; set; }

    public Category Category;

    public EditCategoryModel(){
    }

    public EditCategoryModel(Category category) 
    {
        Init(category);
    }

    public void Init(Category category)
    {
        Category = category;
        Name = category.Name;
        Description = category.Description;
        ParentCategories = (from cat in category.ParentCategories select cat.Name).ToList();
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_350px_square().Url;        
    }

    public Category ConvertToCategory()
    {
        var category = new Category(Name) {ParentCategories = new List<Category>()};
        category.Description = Description;
        foreach (var name in ParentCategories)
            category.ParentCategories.Add(Resolve<CategoryRepository>().GetByName(name));

        var request = HttpContext.Current.Request;

        string categoryType = "standard";

        //if (request["ddlCategoryType"] != null)


        if (request["rdoCategoryTypeGroup"] == "standard")
            categoryType = "Standard";
        if (request["rdoCategoryTypeGroup"] == "media")
            if (request["ddlCategoryTypeMedia"] != null)
                categoryType = request["ddlCategoryTypeMedia"];
        if (request["rdoCategoryTypeGroup"] == "education")
            if (request["ddlCategoryTypeEducation"] != null)
                categoryType = request["ddlCategoryTypeEducation"];

        category.Type = (CategoryType)Enum.Parse(typeof(CategoryType), categoryType);
                

        FillFromRequest(category);

        return category;
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
        category.Description = Description;
        category.ParentCategories.Clear();
        foreach (var name in ParentCategories)
            category.ParentCategories.Add(Resolve<CategoryRepository>().GetByName(name));

        FillFromRequest(category);
    }

    private static void FillFromRequest(Category category)
    {
        var request = HttpContext.Current.Request;

        if (request["WikipediaUrl"] != null)
            category.WikipediaURL = request["WikipediaUrl"];

        if (category.Type == CategoryType.Daily)
            category.TypeJson = new CategoryDaily { ISSN = request["ISSN"], Publisher = request["Publisher"], Url = request["Url"],}.ToJson();


        if (category.Type == CategoryType.WebsiteVideo)
            category.TypeJson = new CategoryWebsiteVideo {Url = request["YoutubeUrl"]}.ToJson();
    }
    
    public void FillReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        ParentCategories = (from key in postData.AllKeys where key.StartsWith("cat-") select postData[key]).ToList();
    }
}
