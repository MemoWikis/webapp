﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Microsoft.Ajax.Utilities;
using NHibernate.Proxy.Poco;
using TrueOrFalse;
using TrueOrFalse.Web;

public class EditCategoryModel : BaseModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public UIMessage Message;

    public IList<Category> ParentCategories = new List<Category>();

    public bool IsEditing { get; set; }

    public string ImageUrl { get; set; }

    public string ImageIsNew { get; set; }
    public string ImageSource { get; set; }
    public string ImageWikiFileName { get; set; }

    public string ImageGuid { get; set; }
    public string ImageLicenceOwner { get; set; }

    public Category Category;

    public string rdoCategoryTypeGroup { get; set; }
    public string ddlCategoryTypeMedia { get; set; }
    public string ddlCategoryTypeEducation { get; set; }

    public EditCategoryModel()
    {
        rdoCategoryTypeGroup = "standard";
    }

    public EditCategoryModel(Category category) : this()
    {
        Init(category);
    }

    public void Init(Category category)
    {
        var parentCategories = category.ParentCategories;
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
        Description = category.Description;
        ParentCategories = parentCategories;
        ImageUrl = new CategoryImageSettings(category.Id).GetUrl_350px_square().Url;
    }

    public ConvertToCategoryResult ConvertToCategory()
    {
        var category = new Category(Name);
        category.Description = Description;
        category.ParentCategories = ParentCategories;

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
        category.ParentCategories = ParentCategories;

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

    private ConvertToCategoryResult FillFromRequest(Category category)
    {
        var request = HttpContext.Current.Request;
        var result = new ConvertToCategoryResult {Category = category};

        if (request["WikipediaUrl"] != null)
            category.WikipediaURL = request["WikipediaUrl"];

        if (category.Type == CategoryType.Book)
            return FillBook(category, request, result);

        if (category.Type == CategoryType.Daily)
            category.TypeJson = new CategoryTypeDaily { ISSN = request["ISSN"], Publisher = request["Publisher"], Url = request["Url"] }.ToJson();

        if (category.Type == CategoryType.DailyIssue)
            return FillDailyIssue(category, request, result);

        if (category.Type == CategoryType.DailyArticle)
            return FillDailyArticle(category, request, result);
        
        if (category.Type == CategoryType.Magazine)
            category.TypeJson = new CategoryTypeMagazine { ISSN = request["ISSN"], Publisher = request["Publisher"], Url = request["Url"] }.ToJson();

        if (category.Type == CategoryType.MagazineIssue)
            return FillMagazineIssue(category, request, result);

        if (category.Type == CategoryType.MagazineArticle)
            return FillMagazineArticle(category, request, result);

        if (category.Type == CategoryType.VolumeChapter)
            return FillVolumeChapter(category, request, result);

        if (category.Type == CategoryType.Website)
            category.TypeJson = new CategoryTypeWebsite { Url = request["Url"] }.ToJson();

        if (category.Type == CategoryType.WebsiteArticle)
            return FillWebsiteArticle(category, request, result);

        if (category.Type == CategoryType.WebsiteVideo)
            category.TypeJson = new CategoryTypeWebsiteVideo {Url = request["YoutubeUrl"]}.ToJson();

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
        
        //category.Name += " (" + category.Type.GetName() + ")";

        return result;
    }

    private ConvertToCategoryResult FillDailyIssue(Category category, HttpRequest request, ConvertToCategoryResult result)
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
                htmlInputName: "hddTxtDaily",
                errorMessage: "Die Ausgabe konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Tageszeitung aus.");

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
            Url = request["Url"],
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
                htmlInputName: "hddTxtDaily",
                errorMessage: "Der Artikel konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Tageszeitung aus.");

        var addParentDailyIssue =
            new AddParentCategoryFromInput(
                category,
                categoryDailyArticle,
                parentCategoryType: CategoryType.DailyIssue,
                htmlInputName: "hddTxtDailyIssue",
                errorMessage: "Der Artikel konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Ausgabe der Tageszeitung aus.");

        if (addParentDaily.HasError)
            return addParentDaily.Result;
        if (addParentDailyIssue.HasError)
            return addParentDailyIssue.Result;
        
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
            PublicationDateMonth = ToNumericalString(request["PublicationDateMonth"]),
            PublicationDateDay = ToNumericalString(request["PublicationDateDay"]),
            Title = request["Title"]
        };

        category.TypeJson = categoryMagazineIssue.ToJson();

        var addParentMagazine =
           new AddParentCategoryFromInput(
               category,
               categoryMagazineIssue,
               parentCategoryType: CategoryType.Magazine,
               htmlInputName: "hddTxtMagazine",
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
            Url = request["Url"],
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
                htmlInputName: "hddTxtMagazine",
                errorMessage: "Der Artikel konnte nicht gespeichert werden. <br>" +
                "Um zu speichern, wähle bitte eine Zeitschrift aus.");

        var addParentMagazineIssue =
            new AddParentCategoryFromInput(
                category,
                categoryMagazineArticle,
                parentCategoryType: CategoryType.MagazineIssue,
                htmlInputName: "hddTxtMagazineIssue",
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
                PublicationDateDay = ToNumericalString(request["PublicationDateDay"]),
                Url = request["Url"]
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

        public AddParentCategoryFromInput(Category category, ICategoryTypeBase typeModel, CategoryType parentCategoryType, string htmlInputName, string errorMessage)
        {
            var request = HttpContext.Current.Request;
            var parentCategoryName = FieldValue = request[htmlInputName];
            var isNullOrEmptyError = String.IsNullOrEmpty(parentCategoryName);

            Category parentFromDb = null;
            if (!isNullOrEmptyError)
                parentFromDb = Sl.Resolve<CategoryRepository>().GetByName(parentCategoryName).First();

            if (isNullOrEmptyError || parentFromDb == null || parentFromDb.Type != parentCategoryType)
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

            category.ParentCategories.Add(parentFromDb);            
        }
    }

    public void FillReleatedCategoriesFromPostData(NameValueCollection postData)
    {
        ParentCategories = RelatedCategoriesUtils.GetReleatedCategoriesFromPostData(postData);
    }
}


public class ConvertToCategoryResult
{
    public Category Category;
    public object TypeModel;

    public bool HasError;
    public UIMessage ErrorMessage;
}