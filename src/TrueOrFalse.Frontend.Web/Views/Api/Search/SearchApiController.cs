using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq.Expressions;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Frontend.Web.Code;

public class SearchApiController : BaseController
{
    public JsonResult ByName(string term, string type)
    {
        var items = new List<ResultItem>();
        var elements = SearchBoxElementsGet.Go(term, type);

        if (elements.Categories.Any())
        {
            AddHeader(items, ResultItemType.CategoriesHeader, elements.CategoriesResultCount, term);
            AddCategoryItems(items, elements);
        }

        if (elements.Questions.Any())
        {
            AddHeader(items, ResultItemType.QuestionsHeader, elements.QuestionsResultCount, term);
            AddQuestionItems(items, elements);
        }

        if (elements.Users.Any())
        {
            AddHeader(items, ResultItemType.UsersHeader, elements.UsersResultCount, term);
            AddUsersItems(items, elements);
        }

        return Json( new{ Items = items }, JsonRequestBehavior.AllowGet);
    }

    public JsonResult ByNameForVue(string term, string type)
    {
        var items = new List<MiniCategoryItem>();
        var elements = SearchBoxElementsGet.GoAllCategories(term);

        if (elements.Categories.Any())
        {
            AddMiniCategoryItems(items, elements);
        }
        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            categories = items,
        }, JsonRequestBehavior.AllowGet);
    }

    public static void AddMiniCategoryItems(List<MiniCategoryItem> items, SearchBoxElements elements)
    {

        items.AddRange(
            elements.Categories.Select(c => new MiniCategoryItem
            {
                Id = c.Id,
                Name = c.Name,
                Url = Links.CategoryDetail(c.Name, c.Id),
                QuestionCount = c.GetCountQuestionsAggregated(),
                ImageUrl = new CategoryImageSettings(c.Id).GetUrl_128px(asSquare: true).Url,
                IconHtml = GetIconHtml(c),
                MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(c.Id, ImageType.Category)).GetImageUrl(30, true, false, ImageType.Category).Url,
                Visibility = (int)c.Visibility
            }));
    }

    public static string GetIconHtml(Category category)
    {
        var iconHTML = "";
        switch (category.Type)
        {
            case CategoryType.Book:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.VolumeChapter:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.Magazine:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineArticle:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineIssue:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.WebsiteArticle:
                iconHTML = "<i class=\"fa fa-globe\">&nbsp;</i>";
                break;
            case CategoryType.Daily:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
            case CategoryType.DailyIssue:
                iconHTML = "<i class=\"fa fa-newspaper-o\"&nbsp;></i>";
                break;
            case CategoryType.DailyArticle:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
        }
        if (category.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education)
            iconHTML = "<i class=\"fa fa-university\">&nbsp;</i>";

        return iconHTML;
    }

    public static string GetIconHtml(CategoryCacheItem category)
    {
        var iconHTML = "";
        switch (category.Type)
        {
            case CategoryType.Book:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.VolumeChapter:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.Magazine:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineArticle:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineIssue:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.WebsiteArticle:
                iconHTML = "<i class=\"fa fa-globe\">&nbsp;</i>";
                break;
            case CategoryType.Daily:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
            case CategoryType.DailyIssue:
                iconHTML = "<i class=\"fa fa-newspaper-o\"&nbsp;></i>";
                break;
            case CategoryType.DailyArticle:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
        }
        if (category.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education)
            iconHTML = "<i class=\"fa fa-university\">&nbsp;</i>";

        return iconHTML;
    }

    private static void AddHeader(List<ResultItem> items, ResultItemType resultItemType, int resultCount, string term)
    {
        string searchUrl = "";

        switch (resultItemType)
        {
            case ResultItemType.QuestionsHeader:
                searchUrl = Links.QuestionSearch(term);
                break;
            case ResultItemType.CategoriesHeader:
                searchUrl = Links.CategoriesSearch(term);
                break;
            case ResultItemType.UsersHeader:
                searchUrl = Links.UsersSearch(term);
                break;
        }


        items.Add(new ResultItem
        {
            ResultCount = resultCount,
            Type = resultItemType.ToString(),
            Item = new ResultSplitter{ SearchUrl = searchUrl }
        });
    }

    private static void AddCategoryItems(List<ResultItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Categories
                .Select(category => new ResultItem
            {
                Type = ResultItemType.Categories.ToString(),
                Item = new ResultItemJson
                {
                    Id = category.Id,
                    Name = category.Name,
                    IconHtml = category.Type.GetCategoryTypeIconHtml(),
                    ImageUrl = new CategoryImageSettings(category.Id).GetUrl_50px(asSquare:true).Url,
                    ItemUrl = Links.CategoryDetail(category.Name, category.Id)
                }
            })
        );
      
    }

    private static void AddQuestionItems(List<ResultItem> items, SearchBoxElements elements)
    {
        var questions = elements.Questions.Where(q => q.IsVisibleToCurrentUser()).ToList();

        var removedQuestionCount = elements.Questions.Count - questions.Count;

        //change header question count
        if (removedQuestionCount > 0)
        {
            var headerItem = items.First(i => i.Type == ResultItemType.QuestionsHeader.ToString());
            headerItem.ResultCount = headerItem.ResultCount - removedQuestionCount;
        }

        items.AddRange(
            questions.Select((question, index) => new ResultItem
            {
                ResultCount = elements.QuestionsResult.Count - removedQuestionCount,
                Type = ResultItemType.Questions.ToString(),
                Item = new ResultItemJson
                {
                    Id = question.Id,
                    Name = question.Text.Wrap(200),
                    ImageUrl = new QuestionImageSettings(question.Id).GetUrl_50px_square().Url,
                    ItemUrl = Links.AnswerQuestion(question, index, "searchbox")
                }
            })
        );
    }

    private static void AddUsersItems(List<ResultItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Users.Select(user => new ResultItem
            {
                ResultCount = elements.UsersResult.Count,
                Type = ResultItemType.Users.ToString(),
                Item = new ResultItemJson
                {
                    Id = user.Id,
                    Name = user.Name,
                    ImageUrl = new UserImageSettings(user.Id).GetUrl_50px_square(user).Url,
                    ItemUrl = Links.UserDetail(user)
                }
            })
        );
    }

    private enum ResultItemType
    {
        CategoriesHeader,
        Categories,
        SetsHeader,
        Sets,
        QuestionsHeader,
        Questions,
        UsersHeader,
        Users
    }

    private class ResultItem
    {
        public int ResultCount;
        public string Type;
        public object Item;
    }

    private class ResultSplitter
    {
        public string SearchUrl;
    }

    public class ResultItemJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconHtml { get; set; }
        public string ImageUrl { get; set; }
        public string ItemUrl { get; set; }
    }
}