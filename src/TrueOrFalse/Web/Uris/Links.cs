using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using TrueOrFalse.Web;

namespace TrueOrFalse.Frontend.Web.Code;

public class Links
{
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public static readonly string AnswerQuestionController = "AnswerQuestion";

    private readonly HttpContext? _httpContext;

    /*Category*/
    public const string CategoryController = "Category";

    /*Question*/
    public const string Questions = "Questions";

    public Links(
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor)
    {
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _httpContext = _httpContextAccessor.HttpContext;
    }

    public string AnswerQuestion(
        Question question,
        int paramElementOnPage = 1,
        string pagerKey = "",
        string categoryFilter = "")
    {
        return AnswerQuestion(question.Text,
            question.Id,
            paramElementOnPage,
            pagerKey,
            categoryFilter);
    }

    public string AnswerQuestion(
        QuestionCacheItem question,
        int paramElementOnPage = 1,
        string pagerKey = "",
        string categoryFilter = "")
    {
        return AnswerQuestion(question.Text,
            question.Id,
            paramElementOnPage,
            pagerKey,
            categoryFilter);
    }

    public string AnswerQuestion(
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

    public string CategoryDetail(Category category)
    {
        return _httpContext == null
            ? ""
            : CategoryDetail(category.Name, category.Id);
    }

    public string CategoryDetail(CategoryCacheItem category)
    {
        return _httpContext == null
            ? ""
            : CategoryDetail(category.Name, category.Id);
    }

    public string CategoryDetail(string name, int id)
    {
        var url = GetUrlHelper()
            .UrlAction("Category", CategoryController, new { text = UriSanitizer.Run(name), id });
        return url;
    }

    public string ErrorNotLoggedIn(string backTo)
    {
        return GetUrlHelper().UrlAction("_NotLoggedIn", "Error", new { backTo });
    }

    public string GetUrl(object type)
    {
        if (type == null)
        {
            return "";
        }

        if (type is Category)
        {
            var topic = (Category)type;
            return GetTopicUrl(UriSanitizer.Run(topic.Name), topic.Id);
        }

        if (type is CategoryCacheItem)
        {
            var topic = (CategoryCacheItem)type;
            return GetTopicUrl(UriSanitizer.Run(topic.Name), topic.Id);
        }

        if (type is Question)
        {
            var question = (Question)type;
            return GetLandingPageUrl(question.Text, question.Id);
        }

        if (type is QuestionCacheItem)
        {
            var question = (QuestionCacheItem)type;
            return GetLandingPageUrl(question.Text, question.Id);
        }

        throw new Exception("unexpected type");
    }

    public IUrlHelper GetUrlHelper()
    {
        var urlHelperFactory = _httpContextAccessor
            .HttpContext?
            .RequestServices
            .GetRequiredService<IUrlHelperFactory>();

        var urlHelper = urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
        urlHelper.RemoveRoutes(new[] { "version" });
        return urlHelper;
    }

    public string QuestionHistory(int questionId)
    {
        return GetUrlHelper().UrlAction("List", "QuestionHistory", new { questionId });
    }

    public string GetLandingPageUrl(string text, int id)
    {
        return $"/Fragen/{text}/{id}";
    }

    public static string UsersSearch(string searchTerm)
    {
        return "/Nutzer/Suche/" + searchTerm;
    }

    public string GetTopicUrl(string text, int id)
    {
        return $"/{text}/{id}";
    }
}