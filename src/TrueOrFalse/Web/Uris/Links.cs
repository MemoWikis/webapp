using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Uris;
using static System.String;

namespace TrueOrFalse.Frontend.Web.Code;

public static class Links
{
    public static readonly string AnswerQuestionController = "AnswerQuestion";
    /*Category*/
    public const string CategoryController = "Category";
    /*Question*/
    public const string Questions = "Questions";
    /*Users*/
    public const string UserAction = "User";
    public const string UserController = "User";

    public static string AnswerQuestion(
        Question question,
        int paramElementOnPage = 1,
        string pagerKey = "",
        string categoryFilter = "")
    {
        return AnswerQuestion(question.Text, question.Id, paramElementOnPage, pagerKey, categoryFilter);
    }

    public static string AnswerQuestion(
        QuestionCacheItem question,
        int paramElementOnPage = 1,
        string pagerKey = "",
        string categoryFilter = "")
    {
        return AnswerQuestion(question.Text, question.Id, paramElementOnPage, pagerKey, categoryFilter);
    }

    public static string AnswerQuestion(Question question)
    {
        return HttpContext.Current == null
            ? ""
            : AnswerQuestion(question, -1);
    }

    public static string AnswerQuestion(
        string questionText,
        int questionId,
        int paramElementOnPage = 1,
        string pagerKey = "",
        string categoryFilter = "")
    {
        if (paramElementOnPage == -1)
        {
            return GetUrlHelper().Action("Answer", AnswerQuestionController,
                new
                {
                    text = UriSegmentFriendlyQuestion.Run(questionText),
                    id = questionId
                }, null);
        }

        return GetUrlHelper().Action("Answer", AnswerQuestionController,
            new
            {
                text = UriSegmentFriendlyQuestion.Run(questionText),
                id = questionId,
                elementOnPage = paramElementOnPage,
                pager = pagerKey,
                category = categoryFilter
            }, null);
    }

    public static string CategoryDetail(Category category)
    {
        return HttpContext.Current == null
            ? ""
            : CategoryDetail(category.Name, category.Id);
    }

    public static string CategoryDetail(CategoryCacheItem category)
    {
        return HttpContext.Current == null
            ? ""
            : CategoryDetail(category.Name, category.Id);
    }

    public static string CategoryDetail(string name, int id)
    {
        return GetUrlHelper().Action("Category", CategoryController,
            new { text = UriSanitizer.Run(name), id });
    }

    public static string ErrorNotLoggedIn(string backTo)
    {
        return GetUrlHelper().Action("_NotLoggedIn", "Error", new { backTo });
    }

    public static string GetUrl(object type)
    {
        if (type == null)
        {
            return "";
        }

        if (type is Category)
        {
            return CategoryDetail((Category)type);
        }

        if (type is CategoryCacheItem)
        {
            return CategoryDetail((CategoryCacheItem)type);
        }

        if (type is Question)
        {
            return AnswerQuestion((Question)type);
        }

        if (type is QuestionCacheItem)
        {
            return AnswerQuestion((QuestionCacheItem)type);
        }

        throw new Exception("unexpected type");
    }

    public static UrlHelper GetUrlHelper()
    {
        var res = new UrlHelper(HttpContext.Current.Request.RequestContext);
        res.RemoveRoutes(new[] { "version" });
        return res;
    }

    public static bool IsLinkToWikipedia(string url)
    {
        if (IsNullOrEmpty(url))
        {
            return false;
        }

        return Regex.IsMatch(url, "https?://.{0,3}wikipedia.");
    }

    public static string QuestionHistory(int questionId)
    {
        return GetUrlHelper().Action("List", "QuestionHistory", new { questionId });
    }

    public static string UserDetail(IUserTinyModel user)
    {
        return UserDetail(user.Name, user.Id);
    }

    public static string UserDetail(string userName, int userId)
    {
        return GetUrlHelper().Action(UserAction, UserController,
            new { name = UriSegmentFriendlyUser.Run(userName), id = userId }, null);
    }
    public static string UsersSearch(string searchTerm)
    {
        return "/Nutzer/Suche/" + searchTerm;
    }
}