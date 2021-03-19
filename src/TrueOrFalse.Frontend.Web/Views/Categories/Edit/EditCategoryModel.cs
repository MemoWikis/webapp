using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using TrueOrFalse.Web;

public class EditCategoryModel : BaseModel
{
    public string Name { get; set; }
    public int Id;

    public string Description { get; set; }

    public UIMessage Message;

    public IList<Category> ParentCategories = new List<Category>();

    public List<Category> DescendantCategories = new List<Category>();

    public IList<Category> AggregatedCategories = new List<Category>();

    public IList<Category> NonAggregatedCategories = new List<Category>();

    public string CategoriesToExcludeIdsString { get; set; }

    public IList<Category> CategoriesToExclude = new List<Category>();

    public string CategoriesToIncludeIdsString { get; set; }

    public IList<Category> CategoriesToInclude = new List<Category>();

    public bool DisableLearningFunctions { get; set; }

    public string TopicMarkdown { get; set; } 

    public bool IsEditing { get; set; }

    public string ImageUrl { get; set; }

    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }

    public string ImageGuid { get; set; }
    public string ImageLicenseOwner { get; set; }

    public Category Category;
    public Category UpdatedCategory; 

    public CategoryType PreselectedType;

    public string rdoCategoryTypeGroup { get; set; }
    public string ddlCategoryTypeMedia { get; set; }
    public string ddlCategoryTypeEducation { get; set; }

    public EditCategoryModel()
    {
        rdoCategoryTypeGroup = "standard";
        PreselectedType = CategoryType.Standard;
        ImageUrl = new CategoryImageSettings(-1).GetUrl_350px_square().Url;
    }

    public EditCategoryModel(Category category) : this()
    {
        Init(category);
    }

    public void Init(Category category)
    {
        var parentCategories = category.ParentCategories();
        if (category.Type == CategoryType.DailyIssue)
            parentCategories = parentCategories.Where(c => c.Type != CategoryType.Daily).ToList();
        if (category.Type == CategoryType.DailyArticle)
            parentCategories = parentCategories.Where(c => c.Type != CategoryType.Daily && c.Type != CategoryType.DailyIssue).ToList();
        if (category.Type == CategoryType.MagazineIssue)
            parentCategories = parentCategories.Where(c => c.Type != CategoryType.Magazine).ToList();
        if (category.Type == CategoryType.MagazineArticle)
            parentCategories = parentCategories.Where(c => c.Type != CategoryType.Magazine && c.Type != CategoryType.MagazineIssue).ToList();


        Category = category;
        Name = category.Name;
        Id = category.Id;
        Description = category.Description;
        ParentCategories = parentCategories;
        AggregatedCategories = category.AggregatedCategories(includingSelf: false).OrderBy(c => c.Name).ToList();
        NonAggregatedCategories = category.NonAggregatedCategories();
        DisableLearningFunctions = category.DisableLearningFunctions;
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_350px_square().Url;
        TopicMarkdown = category.TopicMarkdown;
        CategoriesToIncludeIdsString = category.CategoriesToIncludeIdsString;
        CategoriesToInclude = category.CategoriesToInclude();
        CategoriesToExcludeIdsString = category.CategoriesToExcludeIdsString;
        CategoriesToExclude = category.CategoriesToExclude();
        DescendantCategories = Sl.R<CategoryRepository>().GetDescendants(category.Id).ToList();
    }

    public ConvertToCategoryResult ConvertToCategory()
    {
        var category = new Category(Name);
        category.Description = Description;
        
        category.DisableLearningFunctions = DisableLearningFunctions;
        category.TopicMarkdown = TopicMarkdown;
        ModifyRelationsForCategory.UpdateCategoryRelationsOfType(EntityCache.GetCategoryCacheItem(category.Id), ParentCategories.Select(c => c.Id).ToList(), CategoryRelationType.IsChildCategoryOf);

        var request = HttpContext.Current.Request;
        var categoryType = "standard";

        if (request["rdoCategoryTypeGroup"] == "standard")
            categoryType = "Standard";
        if (request["rdoCategoryTypeGroup"] == "media")
            if (request["ddlCategoryTypeMedia"] != null)
                categoryType = request["ddlCategoryTypeMedia"];
        if (request["rdoCategoryTypeGroup"] == "education")
            if (request["ddlCategoryTypeEducation"] != null)
                categoryType = request["ddlCategoryTypeEducation"];

        category.Type = (CategoryType)Enum.Parse(typeof(CategoryType), categoryType);

        return FillFromRequest(category);
    }

    public void UpdateCategory(Category category)
    {
        category.Name = Name;
        category.Description = Description;

        if (IsInstallationAdmin)//Prevent overwrite of hidden fields if edited by non-admin
        {
            category.DisableLearningFunctions = DisableLearningFunctions;
            category.TopicMarkdown = TopicMarkdown;
        }

        
        ModifyRelationsForCategory.UpdateCategoryRelationsOfType(EntityCache.GetCategoryCacheItem(category.Id), ParentCategories.Select(c => c.Id).ToList(), CategoryRelationType.IsChildCategoryOf);

        UpdatedCategory =  FillFromRequest(category).Category;
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

    private static string ToUrlWithProtocol(string input)
    {
        if (String.IsNullOrEmpty(input?.Trim()))
            return "";

        if (input.ToLower().StartsWith("http://"))
            return "http://" + input.Substring(7);

        if (input.ToLower().StartsWith("https://"))
            return "https://" + input.Substring(8);
        
        return "http://" + input;
    }

    public static string T_ToUrlWithProtocol(string input)
    {
        return ToUrlWithProtocol(input);
    }

    private ConvertToCategoryResult FillFromRequest(Category category)
    {
        var request = HttpContext.Current.Request;
        var result = new ConvertToCategoryResult {Category = category};

        if (request["WikipediaUrl"] != null)
            category.WikipediaURL = ToUrlWithProtocol(request["WikipediaUrl"]);

        if (request["Url"] != null)
            category.Url = ToUrlWithProtocol(request["Url"]);
        category.UrlLinkText = request["UrlLinkText"] ?? "";

        if (category.Type == CategoryType.Book)
            return FillBook(category, request, result);

        if (category.Type == CategoryType.Daily)
            return FillDaily(category, request, result);

        if (category.Type == CategoryType.DailyIssue)
            return FillDailyIssue(category, request, result);

        if (category.Type == CategoryType.DailyArticle)
            return FillDailyArticle(category, request, result);
        
        if (category.Type == CategoryType.Magazine)
            return FillMagazine(category, request, result);

        if (category.Type == CategoryType.MagazineIssue)
            return FillMagazineIssue(category, request, result);

        if (category.Type == CategoryType.MagazineArticle)
            return FillMagazineArticle(category, request, result);

        if (category.Type == CategoryType.VolumeChapter)
            return FillVolumeChapter(category, request, result);

        if (category.Type == CategoryType.WebsiteArticle)
            return FillWebsiteArticle(category, request, result);

        return result;
    }

    private static ConvertToCategoryResult FillBook(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        category.TypeJson =
            new CategoryTypeBook
            {
                Title = request["Title"],
                Subtitle = request["Subtitle"],
                Author = request["Author"],
                ISBN = request["ISBN"],
                Publisher = request["Publisher"],
                PublicationCity = request["PublicationCity"],
                PublicationYear = request["PublicationYear"]
            }.ToJson();
        if (String.IsNullOrEmpty(request["Subtitle"]))
            category.Name = request["Title"];
        else
            category.Name = request["Title"] + " – " + request["Subtitle"];
        
        return result;
    }

    private static ConvertToCategoryResult FillDaily(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        category.TypeJson = new CategoryTypeDaily
        { Title = request["Title"],
            ISSN = request["ISSN"],
            Publisher = request["Publisher"]
        }.ToJson();

        category.Name = request["Title"];

        return result;
    }

    private static ConvertToCategoryResult FillDailyIssue(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        var categoryDailyIssue = new CategoryTypeDailyIssue
        {
            PublicationDateYear = ToNumericalString(request["PublicationDateYear"]),
            Volume = ToNumericalStringWithLeadingZeros(request["Volume"]),
            No = ToNumericalStringWithLeadingZeros(request["No"]),
            PublicationDateMonth = ToNumericalStringWithLeadingZeros(request["PublicationDateMonth"]),
            PublicationDateDay = ToNumericalStringWithLeadingZeros(request["PublicationDateDay"]),
            Category = category
        };

        category.TypeJson = categoryDailyIssue.ToJson();
        
        var addParentDaily = 
            new AddParentCategoryFromInput(
                category, 
                categoryDailyIssue,
                parentCategoryType: CategoryType.Daily,
                htmlInputId: "hddTxtDaily",
                errorMessage: "Die Ausgabe konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Zeitung aus.");

        category.Name = categoryDailyIssue.BuildTitle(addParentDaily.FieldValue);

        return addParentDaily.HasError ? addParentDaily.Result : result;
    }

    private static ConvertToCategoryResult FillDailyArticle(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        var categoryDailyArticle = new CategoryTypeDailyArticle
        {
            Title = request["Title"],
            Subtitle = request["Subtitle"],
            Author = request["Author"],
            PagesArticleFrom = request["PagesArticleFrom"],
            PagesArticleTo = request["PagesArticleTo"]
        };

        category.TypeJson = categoryDailyArticle.ToJson();

        if (String.IsNullOrEmpty(request["Subtitle"]))
            category.Name = request["Title"];
        else
            category.Name = request["Title"] + " – " + request["Subtitle"];

        var addParentDaily =
            new AddParentCategoryFromInput(
                category,
                categoryDailyArticle,
                parentCategoryType: CategoryType.Daily,
                htmlInputId: "hddTxtDaily",
                errorMessage: "Der Artikel konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Zeitung aus.");

        var addParentDailyIssue =
            new AddParentCategoryFromInput(
                category,
                categoryDailyArticle,
                parentCategoryType: CategoryType.DailyIssue,
                htmlInputId: "hddTxtDailyIssue",
                errorMessage: "Der Artikel konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Ausgabe der Zeitung aus.");

        if (addParentDaily.HasError)
            return addParentDaily.Result;
        if (addParentDailyIssue.HasError)
            return addParentDailyIssue.Result;
        
        return result;
    }

    private static ConvertToCategoryResult FillMagazine(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        category.TypeJson = new CategoryTypeMagazine
        {
            Title = request["Title"],
            ISSN = request["ISSN"],
            Publisher = request["Publisher"]
        }.ToJson();

        category.Name = request["Title"];

        return result;
    }

    private static ConvertToCategoryResult FillMagazineIssue(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        var categoryMagazineIssue = new CategoryTypeMagazineIssue
        {
            PublicationDateYear = ToNumericalString(request["PublicationDateYear"]),
            Volume = ToNumericalString(request["Volume"]),
            No = ToNumericalStringWithLeadingZeros(request["No"]),
            IssuePeriod = request["IssuePeriod"],
            PublicationDateMonth = ToNumericalStringWithLeadingZeros(request["PublicationDateMonth"]),
            PublicationDateDay = ToNumericalStringWithLeadingZeros(request["PublicationDateDay"]),
            Title = request["Title"]
        };

        category.TypeJson = categoryMagazineIssue.ToJson();

        var addParentMagazine =
           new AddParentCategoryFromInput(
               category,
               categoryMagazineIssue,
               parentCategoryType: CategoryType.Magazine,
               htmlInputId: "hddTxtMagazine",
               errorMessage: "Die Ausgabe konnte nicht gespeichert werden. <br>" +
               "Um zu speichern, wähle bitte eine Zeitschrift aus.");

        category.Name = categoryMagazineIssue.BuildTitle(addParentMagazine.FieldValue);


        return addParentMagazine.HasError ? addParentMagazine.Result : result;
    }

    private static ConvertToCategoryResult FillMagazineArticle(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        var categoryMagazineArticle = new CategoryTypeMagazineArticle
        {
            Title = request["Title"],
            Subtitle = request["Subtitle"],
            Author = request["Author"],
            PagesArticleFrom = request["PagesArticleFrom"],
            PagesArticleTo = request["PagesArticleTo"]
        };

        category.TypeJson = categoryMagazineArticle.ToJson();

        if (String.IsNullOrEmpty(request["Subtitle"]))
            category.Name = request["Title"];
        else
            category.Name = request["Title"] + " – " + request["Subtitle"];

        var addParentMagazine =
            new AddParentCategoryFromInput(
                category,
                categoryMagazineArticle,
                parentCategoryType: CategoryType.Magazine,
                htmlInputId: "hddTxtMagazine",
                errorMessage: "Der Artikel konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Zeitschrift aus.");

        var addParentMagazineIssue =
            new AddParentCategoryFromInput(
                category,
                categoryMagazineArticle,
                parentCategoryType: CategoryType.MagazineIssue,
                htmlInputId: "hddTxtMagazineIssue",
                errorMessage: "Der Artikel konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Ausgabe der Zeitschrift aus.");

        if (addParentMagazine.HasError)
            return addParentMagazine.Result;
        if (addParentMagazineIssue.HasError)
            return addParentMagazineIssue.Result;

        return result;
    }

    private static ConvertToCategoryResult FillVolumeChapter(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        category.TypeJson =
            new CategoryTypeVolumeChapter
            {
                Title = request["Title"],
                Subtitle = request["Subtitle"],
                Author = request["Author"],
                TitleVolume = request["TitleVolume"],
                SubtitleVolume = request["SubtitleVolume"],
                Editor = request["Editor"],
                ISBN = request["ISBN"],
                Publisher = request["Publisher"],
                PublicationCity = request["PublicationCity"],
                PublicationYear = request["PublicationYear"],
                PagesChapterFrom = request["PagesChapterFrom"],
                PagesChapterTo = request["PagesChapterTo"]
            }.ToJson();
        if (String.IsNullOrEmpty(request["Subtitle"]))
            category.Name = request["Title"];
        else
            category.Name = request["Title"] + " – " + request["Subtitle"];

        return result;
    }

    private static ConvertToCategoryResult FillWebsiteArticle(Category category, HttpRequest request, ConvertToCategoryResult result)
    {
        category.TypeJson =
            new CategoryTypeWebsiteArticle
            {
                Title = request["Title"],
                Subtitle = request["Subtitle"],
                Author = request["Author"],
                PublicationDateYear = ToNumericalString(request["PublicationDateYear"]),
                PublicationDateMonth = ToNumericalString(request["PublicationDateMonth"]),
                PublicationDateDay = ToNumericalString(request["PublicationDateDay"])
            }.ToJson();

        if (String.IsNullOrEmpty(request["Subtitle"]))
            category.Name = request["Title"];
        else
            category.Name = request["Title"] + " – " + request["Subtitle"];

        return result;
    }

    private class AddParentCategoryFromInput
    {
        public bool HasError { get { return Result != null; } }
        public readonly ConvertToCategoryResult Result;
        public readonly string FieldValue;

        public AddParentCategoryFromInput(Category category, ICategoryTypeBase typeModel, CategoryType parentCategoryType, string htmlInputId, string errorMessage)
        {
            var request = HttpContext.Current.Request;
            var parentCategoryIdText = FieldValue = request[htmlInputId];
            var parentCategoryId = -1;
            var isError = (String.IsNullOrEmpty(parentCategoryIdText) || !Int32.TryParse(parentCategoryIdText, out parentCategoryId));

            Category parentFromDb = null;
            if (!isError)
                parentFromDb = Sl.Resolve<CategoryRepository>().GetById(parentCategoryId);

            if (isError || parentFromDb == null || parentFromDb.Type != parentCategoryType)
            {
                {
                    Result = new ConvertToCategoryResult
                    {
                        TypeModel = typeModel,
                        Category = category,
                        HasError = true,
                        ErrorMessage = new ErrorMessage(errorMessage)
                    };

                    return;
                }
            }

            ModifyRelationsForCategory.AddParentCategory(EntityCache.GetCategoryCacheItem(category.Id), parentFromDb.Id);            
        }
    }

    public void FillReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        ParentCategories = AutocompleteUtils.GetRelatedCategoriesFromPostData(postData);
    }

    public bool IsInCategoriesToInclude(int categoryId)
    {
        return !string.IsNullOrEmpty(CategoriesToIncludeIdsString) 
            && CategoriesToIncludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x))
            .Any(c => c == categoryId);
    } 
}


public class ConvertToCategoryResult
{
    public Category Category;
    public object TypeModel;

    public bool HasError;
    public UIMessage ErrorMessage;
}