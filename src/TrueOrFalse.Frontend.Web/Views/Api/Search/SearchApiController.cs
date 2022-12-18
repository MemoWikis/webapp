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
    [HttpGet]
    public JsonResult ByName(string term, string type)
    {
        var categoryItems = new List<SearchCategoryItem>();
        var questionItems = new List<SearchQuestionItem>();
        var userItems = new List<SearchUserItem>();
        var elements = SearchBoxElementsGet.Go(term, type);

        if (elements.Categories.Any())
            AddCategoryItems(categoryItems, elements);

        if (elements.Questions.Any())
            AddQuestionItems(questionItems, elements);

        if (elements.Users.Any())
            AddUserItems(userItems, elements);

        return Json(new
        {
            categories = categoryItems,
            categoryCount = elements.CategoriesResultCount,
            questions = questionItems,
            questionCount = elements.QuestionsResultCount,
            users = userItems,
            userCount = elements.UsersResultCount,
            userSearchUrl = Links.UsersSearch(term)
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult Category(string term, int[] categoriesToFilter)
    {
        var items = new List<SearchCategoryItem>();
        var elements = SearchBoxElementsGet.GoAllCategories(term, categoriesToFilter);

        if (elements.Categories.Any())
            AddCategoryItems(items, elements);

        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            categories = items,
        }, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CategoryInWiki(string term, int[] categoriesToFilter)
    {
        var items = new List<SearchCategoryItem>();
        var elements = SearchBoxElementsGet.GoAllCategories(term, categoriesToFilter);

        if (elements.Categories.Any())
            AddCategoryItems(items, elements);

        var wikiChildren = EntityCache.GetAllChildren(SessionUser.User.StartTopicId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            categories = items,
        }, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult GetPersonalWikiData(int id)
    {

        if (EntityCache.GetAllChildren(id).Any(c => c.Id == SessionUser.User.StartTopicId))
            return Json(new
            {
                success = false,
            });

        var personalWiki = EntityCache.GetCategory(SessionUser.User.StartTopicId);
        var personalWikiItem = FillSearchCategoryItem(personalWiki);
        var recentlyUsedRelationTargetTopics = new List<SearchCategoryItem>();
        
        if (SessionUser.User.RecentlyUsedRelationTargetTopicIds != null && SessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var categoryId in SessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var c = EntityCache.GetCategory(categoryId);
                recentlyUsedRelationTargetTopics.Add(FillSearchCategoryItem(c));
            }
        }

        return Json(new
        {
            success = true,
            personalWiki = personalWikiItem,
            addToWikiHistory = recentlyUsedRelationTargetTopics.ToArray()
        });
    }

    public static void AddCategoryItems(List<SearchCategoryItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Categories.Where(PermissionCheck.CanView).Select(FillSearchCategoryItem));
    }

    public static SearchCategoryItem FillSearchCategoryItem(Category c)
    {
        return new SearchCategoryItem
        {
            Id = c.Id,
            Name = c.Name,
            Url = Links.CategoryDetail(c.Name, c.Id),
            QuestionCount = EntityCache.GetCategory(c.Id).GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(c.Id).GetUrl_128px(asSquare: true).Url,
            IconHtml = GetIconHtml(c),
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(c.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)c.Visibility
        };
    }

    public static SearchCategoryItem FillSearchCategoryItem(CategoryCacheItem c)
    {
        return new SearchCategoryItem
        {
            Id = c.Id,
            Name = c.Name,
            Url = Links.CategoryDetail(c.Name, c.Id),
            QuestionCount = EntityCache.GetCategory(c.Id).GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(c.Id).GetUrl_128px(asSquare: true).Url,
            IconHtml = GetIconHtml(c),
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(c.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)c.Visibility
        };
    }

    public static void AddQuestionItems(List<SearchQuestionItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Questions.Where(q => PermissionCheck.CanView(q)).Select((q, index) => new SearchQuestionItem
            {
                Id = q.Id,
                Name = q.Text.Wrap(200),
                ImageUrl = new QuestionImageSettings(q.Id).GetUrl_50px_square().Url,
                Url = Links.AnswerQuestion(q, index, "searchbox")
            }));
    }

    public static void AddUserItems(List<SearchUserItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Users.Select(u => new SearchUserItem
            {
                Id = u.Id,
                Name = u.Name,
                ImageUrl = new UserImageSettings(u.Id).GetUrl_50px_square(u).Url,
                Url = Links.UserDetail(u)
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

    private static void AddQuestionItems(List<ResultItem> items, SearchBoxElements elements)
    {
        var questions = elements.Questions.Where(q => PermissionCheck.CanView(q)).ToList();

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