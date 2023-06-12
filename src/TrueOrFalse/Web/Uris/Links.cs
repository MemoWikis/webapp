using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Uris;
using static System.String;

namespace TrueOrFalse.Frontend.Web.Code;

public static class Links
{
    /* About */
    public const string AboutController = "About";
    public const string AccountController = "Account";

    /* AlgoInsight */
    public const string AlgoInsightController = "AlgoInsight";

    public const string AnswerQuestionController = "AnswerQuestion";

    /* Games */

    /*Category*/
    public const string CategoryController = "Category";
    public const string CategoryCreateAction = "Create";
    public const string CategoryEditController = "EditCategory";
    public const string EditQuestionController = "EditQuestion";
    public const string HelpActionFAQ = "FAQ";
    public const string HelpController = "Help";
    /*Learn*/
    public const string NetworkAction = "Network";
    /*Question*/
    public const string Questions = "Questions";
    public const string RegisterAction = "Register";
    public const string RegisterController = "Register";
    public const string SetController = "Set";
    /*Users*/
    public const string UserAction = "User";
    public const string UserController = "User";
    public const string UsersAction = "Users";
    public const string UsersController = "Users";
    public const string UserSettingsAction = "UserSettings";
    public const string UserSettingsController = "UserSettings";

    public const string VariousController = "VariousPublic";
    public const string WelcomeController = "Welcome";

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

    public static string CategoryDetailLearningTab(string name, int id)
    {
        return CategoryDetail(name, id) + "/Lernen";
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

        if (type is Set)
        {
            return SetDetail((Set)type);
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

    public static string LearningSession(LearningSession learningSession)
    {
        return "#";
    }

    public static string QuestionHistory(int questionId)
    {
        return GetUrlHelper().Action("List", "QuestionHistory", new { questionId });
    }

    public static string Register()
    {
        return GetUrlHelper().Action(RegisterAction, RegisterController);
    }

    public static string SetDetail(UrlHelper url, Set set)
    {
        return SetDetail(url, set.Name, set.Id);
    }

    public static string SetDetail(Set set)
    {
        return HttpContext.Current == null
            ? ""
            : SetDetail(set.Name, set.Id);
    }

    public static string SetDetail(string name, int id)
    {
        return SetDetail(GetUrlHelper(), name, id);
    }

    public static string SetDetail(UrlHelper url, string name, int id)
    {
        return url.Action("QuestionSet", "Set",
            new { text = UriSanitizer.Run(name), id }, null);
    }

    public static string SetsSearch(string searchTerm)
    {
        return "/Fragesaetze/Suche/" + searchTerm;
    }

    public static string StartCategoryLearningSession(int categoryId)
    {
        return GetUrlHelper().Action("StartLearningSession", CategoryController, new { categoryId });
    }

    public static string StartLearningSession(LearningSession learningSession)
    {
        StartCategoryLearningSession(learningSession.Config.CategoryId);
        return StartCategoryLearningSession(learningSession.Config.CategoryId);
    }

    public static string Team()
    {
        return GetUrlHelper().Action("Team", WelcomeController);
    }

    public static string TestSession(string categoryName, int categoryId)
    {
        return CategoryDetailLearningTab(categoryName, categoryId);
    }

    public static string TestSessionStartForCategory(string categoryName, int categoryId)
    {
        return GetUrlHelper().Action("StartTestSession", CategoryController,
            new { categoryName = UriSanitizer.Run(categoryName), categoryId });
    }

    public static string TestSessionStartForSet(string setName, int setId)
    {
        return GetUrlHelper().Action("StartTestSession", SetController,
            new { setName = UriSanitizer.Run(setName), setId });
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

    public static string Users()
    {
        return GetUrlHelper().Action(UsersAction, UsersController);
    }

    public static string UserSettings()
    {
        return GetUrlHelper().Action(UserSettingsAction, UserSettingsController);
    }

    public static string UsersSearch(string searchTerm)
    {
        return "/Nutzer/Suche/" + searchTerm;
    }

    /* Welcome */
    public static string Welcome()
    {
        return GetUrlHelper().Action("Welcome", WelcomeController);
    }

    public static string WelcomeLinks(string name, int Id)
    {
        return "/" + name + "/" + Id;
    }

    public static string WidgetStats()
    {
        return GetUrlHelper().Action("WidgetStats", AccountController);
    }
}