using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using TrueOrFalse;
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

    private static string ToNumericalString(string input)
    {
        int outInt;
        return !Int32.TryParse(input, out outInt) ? String.Empty : outInt.ToString();
    }

    private static string ToNumericalStringWithLeadingZeros(string input)
    {
        int outInt;
        return !Int32.TryParse(input, out outInt) ? String.Empty : input;
    }

    private static void FillFromRequest(Category category)
    {
        var request = HttpContext.Current.Request;

        if (request["WikipediaUrl"] != null)
            category.WikipediaURL = request["WikipediaUrl"];

        if (category.Type == CategoryType.Book)
        {
            category.TypeJson = new CategoryBook { Title = request["Title"], Subtitle = request["Subtitle"], Author = request["Author"], ISBN = request["ISBN"], Publisher = request["Publisher"], PublicationCity = request["PublicationCity"], PublicationYear = request["PublicationYear"] }.ToJson();
            if (String.IsNullOrEmpty(request["Subtitle"]))
                category.Name = request["Title"];
            else
                category.Name = request["Title"] + " – " + request["Subtitle"];
        }
        
        if (category.Type == CategoryType.Daily)
            category.TypeJson = new CategoryDaily { ISSN = request["ISSN"], Publisher = request["Publisher"], Url = request["Url"] }.ToJson();

        if (category.Type == CategoryType.DailyIssue)
        {
            var categoryDailyIssue = new CategoryDailyIssue
            {
                Year = ToNumericalString(request["Year"]),
                Volume = ToNumericalString(request["Volume"]),
                No = ToNumericalStringWithLeadingZeros(request["No"]),
                PublicationDateMonth = ToNumericalString(request["PublicationDateMonth"]),
                PublicationDateDay = ToNumericalString(request["PublicationDateDay"])
            };
            
            category.TypeJson = categoryDailyIssue.ToJson();

            category.Name = categoryDailyIssue.BuildTitle();
        }

        if (category.Type == CategoryType.DailyArticle)
        {
            category.TypeJson = new CategoryDailyArticle { Title = request["Title"], Subtitle = request["Subtitle"], Author = request["Author"], Url = request["Url"] }.ToJson();
            if (String.IsNullOrEmpty(request["Subtitle"]))
                category.Name = request["Title"];
            else
                category.Name = request["Title"] + " – " + request["Subtitle"];
        }

        if (category.Type == CategoryType.Magazine)
            category.TypeJson = new CategoryMagazine { ISSN = request["ISSN"], Publisher = request["Publisher"], Url = request["Url"] }.ToJson();

        if (category.Type == CategoryType.MagazineIssue)
        {
            var categoryMagazineIssue = new CategoryMagazineIssue
            {
                Year = ToNumericalString(request["Year"]),
                Volume = ToNumericalString(request["Volume"]),
                No = ToNumericalStringWithLeadingZeros(request["No"]),
                IssuePeriod = request["IssuePeriod"],
                PublicationDateMonth = ToNumericalString(request["PublicationDateMonth"]),
                PublicationDateDay = ToNumericalString(request["PublicationDateDay"]),
                Title = request["Title"]
            };

            category.TypeJson = categoryMagazineIssue.ToJson();

            category.Name = categoryMagazineIssue.BuildTitle();
        }

        if (category.Type == CategoryType.VolumeChapter)
        {
            category.TypeJson = new CategoryVolumeChapter { Title = request["Title"], Subtitle = request["Subtitle"], Author = request["Author"], TitleVolume = request["TitleVolume"], SubtitleVolume = request["SubtitleVolume"], Editor = request["Editor"], ISBN = request["ISBN"], Publisher = request["Publisher"], PublicationCity = request["PublicationCity"], PublicationYear = request["PublicationYear"], PagesChapterFrom = request["PagesChapterFrom"], PagesChapterTo = request["PagesChapterTo"] }.ToJson();
            if (String.IsNullOrEmpty(request["Subtitle"]))
                category.Name = request["Title"];
            else
                category.Name = request["Title"] + " – " + request["Subtitle"];
        }
        
        if (category.Type == CategoryType.Website)
            category.TypeJson = new CategoryWebsite { Url = request["Url"] }.ToJson();

        if (category.Type == CategoryType.WebsiteVideo)
            category.TypeJson = new CategoryWebsiteVideo {Url = request["YoutubeUrl"]}.ToJson();
    }
    
    public void FillReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        ParentCategories = (from key in postData.AllKeys where key.StartsWith("cat-") select postData[key]).ToList();
    }
}
