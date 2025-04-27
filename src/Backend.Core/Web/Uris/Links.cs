using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

public class Links
{
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public static readonly string AnswerQuestionController = "AnswerQuestion";

    private readonly HttpContext? _httpContext;

    /*Category*/
    public const string PageController = "Page";

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
        string pageToFilter = "")
    {
        return AnswerQuestion(question.Text,
            question.Id,
            paramElementOnPage,
            pagerKey,
            pageToFilter);
    }

    public string AnswerQuestion(
        QuestionCacheItem question,
        int paramElementOnPage = 1,
        string pagerKey = "",
        string pageToFilter = "")
    {
        return AnswerQuestion(question.Text,
            question.Id,
            paramElementOnPage,
            pagerKey,
            pageToFilter);
    }

    public string AnswerQuestion(
        string questionText,
        int questionId,
        int paramElementOnPage = 1,
        string pagerKey = "",
        string pageToFilter = "")
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
                page = pageToFilter
            }, null);
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

        if (type is Page)
        {
            var page = (Page)type;
            return GetPageUrl(UriSanitizer.Run(page.Name), page.Id);
        }

        if (type is PageCacheItem)
        {
            var page = (PageCacheItem)type;
            return GetPageUrl(UriSanitizer.Run(page.Name), page.Id);
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

    public string GetPageUrl(string text, int id)
    {
        return $"/{text}/{id}";
    }
}