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
    public const string CategoriesAction = "Categories";
    public const string CategoriesController = "Categories";
    public const string CategoriesWishAction = "CategoriesWish";
    public const string CategoryController = "Category";
    public const string CategoryCreateAction = "Create";
    public const string CategoryEditController = "EditCategory";
    public const string CategoryNewController = "CategoryNew";
    public const string EditQuestionController = "EditQuestion";
    public const string HelpActionFAQ = "FAQ";

    public const string HelpController = "Help";
    public const string HelpWillkommen = "Willkommen";
    public const string HelpWunschwissen = "Willkommen";
    public const string KnowledgeAction = "Knowledge";

    public const string KnowledgeController = "Knowledge";

    /*Learn*/
    public const string LearningSessionResultController = "LearningSessionResult";
    public const string Logout = "Logout";
    public const string NetworkAction = "Network";

    /* Images*/
    public const string NoCategoryUrl = "/Images/no-category-picture-350.png";
    public const string NoQuestionUrl = "/Images/no-question-128.png";


    /*Question*/
    public const string Questions = "Questions";
    public const string QuestionsMineAction = "QuestionsMine";
    public const string QuestionsWishAction = "QuestionsWish";
    public const string RegisterAction = "Register";
    public const string RegisterController = "Register";
    public const string RegisterSuccess = "RegisterSuccess";
    public const string SetController = "Set";
    public const string SetCreateAction = "Create";
    public const string SetEditController = "EditSet";
    public const string SetsAction = "Sets";

    /*Questionsets / Sets*/
    public const string SetsController = "Sets";
    public const string SetsMineAction = "SetsMine";
    public const string SetsWishAction = "SetsWish";
    public const string UserAction = "User";

    /*Users*/
    public const string UserController = "User";
    public const string UsersAction = "Users";

    public const string UsersController = "Users";

    public const string UserSettingsAction = "UserSettings";
    public const string UserSettingsController = "UserSettings";

    public const string VariousController = "VariousPublic";
    public const string WelcomeController = "Welcome";


    // Partner

    public static string Tutory = "https://www.tutory.de";
    public static string TutoryImage = "/Images/LogosPartners/Logo_tutory_250px.png";
    public static string Contact => GetUrlHelper().Action("Contact", "Welcome");
    public static string Imprint => GetUrlHelper().Action("Imprint", VariousController);
    public static string TermsAndConditions => GetUrlHelper().Action("TermsAndConditions", VariousController);

    public static string AlgoInsightForecast()
    {
        return GetUrlHelper().Action("Forecast", AlgoInsightController);
    }

    public static string AnalyticsFooter(int categoryId, string categoryName)
    {
        return "/" + categoryName + "/" + categoryId + "/Wissensnetz";
    }

    public static string AnswerQuestion(Question question, Set set)
    {
        return AnswerQuestion(GetUrlHelper(), question, set);
    }

    public static string AnswerQuestion(UrlHelper url, Question question, Set set)
    {
        return url.Action("Answer", AnswerQuestionController,
            new { text = UriSegmentFriendlyQuestion.Run(question.Text), questionId = question.Id, setId = set.Id });
    }

    public static string AnswerQuestion(UrlHelper url, string questionText, int questionId, int setId)
    {
        return url.Action("Answer", AnswerQuestionController,
            new { text = UriSegmentFriendlyQuestion.Run(questionText), questionId, setId });
    }

    public static string AnswerQuestion(QuestionSearchSpec searchSpec)
    {
        return "/AnswerQuestion/Answer?pager=" + searchSpec?.Key;
    }

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

    public static string BetaInfo()
    {
        return GetUrlHelper().Action("MemuchoBeta", VariousController);
    }

    public static string CategoriesSearch(string searchTerm)
    {
        return "/Suche/" + searchTerm;
    }

    public static string CategoryChangesOverview(int pageToShow)
    {
        return GetUrlHelper().Action("List", "CategoryChangesOverview", new { pageToShow });
    }

    public static string CategoryCreate()
    {
        return GetUrlHelper().Action(CategoryCreateAction, CategoryEditController);
    }

    public static string CategoryCreate(int parentCategoryId)
    {
        return GetUrlHelper().Action("Create", "EditCategory", new { parent = parentCategoryId });
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

    public static string CategoryDetail(CategoryCacheItem category, int version)
    {
        return HttpContext.Current == null
            ? ""
            : CategoryDetail(category.Name, category.Id, version);
    }

    public static string CategoryDetail(string name, int id)
    {
        return GetUrlHelper().Action("Category", CategoryController,
            new { text = UriSanitizer.Run(name), id });
    }

    public static string CategoryDetail(string name, int id, int version)
    {
        return GetUrlHelper().Action("Category", CategoryController,
            new { text = UriSanitizer.Run(name), id, version }, null);
    }

    public static string CategoryDetailAnalyticsTab(CategoryCacheItem category)
    {
        return CategoryDetail(category) + "/Wissensnetz";
    }

    public static string CategoryDetailAnalyticsTab(string name, int id)
    {
        return CategoryDetail(name, id) + "/Wissensnetz";
    }

    public static string CategoryDetailLearningTab(string name, int id)
    {
        return CategoryDetail(name, id) + "/Lernen";
    }

    public static string CategoryDetailLearningTab(CategoryCacheItem category)
    {
        return CategoryDetail(category) + "/Lernen";
    }


    public static string CategoryDetailRedirect(string name, int id)
    {
        return GetUrlHelper().Action("Category", "Category", new { text = UriSanitizer.Run(name), id });
    }

    public static string CategoryEdit(CategoryCacheItem categoryCacheItem)
    {
        return CategoryEdit(GetUrlHelper(), categoryCacheItem.Name, categoryCacheItem.Id);
    }

    public static string CategoryEdit(Category categoryCacheItem)
    {
        return CategoryEdit(GetUrlHelper(), categoryCacheItem.Name, categoryCacheItem.Id);
    }

    public static string CategoryEdit(string name, int id)
    {
        return CategoryEdit(GetUrlHelper(), name, id);
    }

    public static string CategoryEdit(UrlHelper url, string name, int id)
    {
        return url.Action("Edit", "EditCategory", new { text = UriSanitizer.Run(name), id });
    }

    public static string CategoryFromNetwork(CategoryCacheItem category)
    {
        return CategoryFromNetwork(category.Name, category.Id);
    }

    public static string CategoryFromNetwork(string name, int id)
    {
        return GetUrlHelper().Action("Category", CategoryController,
            new { text = UriSanitizer.Run(name), id, toRootCategory = true, isFromNetwork = true });
    }

    public static string CategoryHistory(int categoryId)
    {
        return GetUrlHelper().Action("List", "CategoryHistory", new { categoryId });
    }

    public static string CategoryHistoryDetail(int categoryId, int categoryChangeId)
    {
        return GetUrlHelper().Action("Detail", "CategoryHistoryDetail", new { categoryId, categoryChangeId });
    }

    public static string CategoryHistoryDetail(int categoryId, int firstEditId, int selectedRevId)
    {
        return GetUrlHelper().Action("GroupedDetail", "CategoryHistoryDetail",
            new { categoryId, firstEditId, selectedRevId });
    }

    public static string CategoryRestore(int categoryId, int categoryChangeId)
    {
        return GetUrlHelper().Action("Restore", CategoryEditController, new { categoryId, categoryChangeId });
    }

    public static string CCLicenses(string imageName)
    {
        return "/Images/Licenses/" + imageName;
    }

    public static string CountLastAnswerAsCorrect(UrlHelper url, QuestionCacheItem question)
    {
        return url.Action("CountLastAnswerAsCorrect", AnswerQuestionController, new { id = question.Id }, null);
    }

    public static string CountUnansweredAsCorrect(UrlHelper url, QuestionCacheItem question)
    {
        return url.Action("CountUnansweredAsCorrect", AnswerQuestionController, new { id = question.Id }, null);
    }

    public static string CreateQuestion(int categoryId = -1, int setId = -1)
    {
        var url = GetUrlHelper();

        if (categoryId != -1)
        {
            return url.Action("Create", EditQuestionController, new { categoryId });
        }

        if (setId != -1)
        {
            return url.Action("Create", EditQuestionController, new { setId });
        }

        return url.Action("Create", EditQuestionController);
    }

    public static string EditQuestion(Question question)
    {
        return EditQuestion(GetUrlHelper(), question.Text, question.Id);
    }

    public static string EditQuestion(string questionText, int questionId)
    {
        return EditQuestion(GetUrlHelper(), questionText, questionId);
    }

    public static string EditQuestion(UrlHelper url, string questionText, int questionId)
    {
        return url.Action("Edit", EditQuestionController,
            new { text = UriSanitizer.Run(questionText), id = questionId });
    }

    public static string Error500(string backTo)
    {
        return GetUrlHelper().Action("_500", "Error", new { backTo });
    }

    public static string ErrorNotLoggedIn(string backTo)
    {
        return GetUrlHelper().Action("_NotLoggedIn", "Error", new { backTo });
    }


    public static string FAQItem(string itemNameInView)
    {
        return GetUrlHelper().Action("FAQ", "Help") + "#" + itemNameInView;
    }

    public static string GetSolution(UrlHelper url, QuestionCacheItem question)
    {
        return url.Action("GetSolution", AnswerQuestionController, new { id = question.Id }, null);
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

    public static string GoogleMapsPreviewPath(string imageName)
    {
        return "/Images/GoogleMapsPreview/" + imageName;
    }

    public static string HelpFAQ()
    {
        return GetUrlHelper().Action(HelpActionFAQ, HelpController);
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

    public static string LearningSessionAmendAfterShowSolution(UrlHelper url)
    {
        return url.Action("AmendAfterShowSolution", AnswerQuestionController);
    }

    /* Category Footer*/
    public static string LearningSessionFooter(int categoryId, string categoryName)
    {
        return "/" + categoryName + "/" + categoryId + "/Lernen";
    }

    public static string MessageSetRead(UrlHelper url)
    {
        return url.Action("SetMessageRead", "Messages");
    }

    public static object MessageSetUnread(UrlHelper url)
    {
        return url.Action("SetMessageUnread", "Messages");
    }

    public static string Network()
    {
        return GetUrlHelper().Action(NetworkAction, UsersController);
    }

    public static string Promoter()
    {
        return GetUrlHelper().Action("Promoter", WelcomeController);
    }

    public static string QuestionChangesOverview(int pageToShow)
    {
        return GetUrlHelper().Action("List", "QuestionChangesOverview", new { pageToShow });
    }

    public static string QuestionHistory(int questionId)
    {
        return GetUrlHelper().Action("List", "QuestionHistory", new { questionId });
    }

    public static string QuestionHistoryDetail(int questionId, int revisionId)
    {
        return GetUrlHelper().Action("Detail", "QuestionHistoryDetail", new { questionId, revisionId });
    }

    public static string QuestionRestore(int questionId, int questionChangeId)
    {
        return GetUrlHelper().Action("Restore", "Question", new { questionId, questionChangeId });
    }

    public static string QuestionSearch(string searchTerm)
    {
        return "/Fragen/Suche/" + searchTerm;
    }

    public static string QuestionWish_WithCategoryFilter(CategoryCacheItem category)
    {
        return "/Fragen/Wunschwissen/Suche/Kategorie/" + UriSanitizer.Run(category.Name) + "/" + category.Id;
    }

    public static string QuestionWithCategoryFilter(UrlHelper url, MenuModelCategoryItem modelCategoryItem)
    {
        return modelCategoryItem.SearchUrl + "Kat__" + modelCategoryItem.Category.Name + "__";
    }

    public static string QuestionWithCategoryFilter(UrlHelper url, Category category)
    {
        return "/Fragen/Suche/Kategorie/" + UriSanitizer.Run(category.Name) + "/" + category.Id;
    }

    public static string QuestionWithCategoryFilter(UrlHelper url, string categoryName, int categoryId)
    {
        return "/Fragen/Suche/Kategorie/" + UriSanitizer.Run(categoryName) + "/" + categoryId;
    }

    public static string QuestionWithCreatorFilter(UrlHelper url, User user)
    {
        return user != null
            ? "/Fragen/Suche/" + "Ersteller__" + user.Name + "__"
            : "/Fragen/Suche/" + "Ersteller__unbekannt__";
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

    public static string UserLoginAs(UrlHelper url, int userId)
    {
        return url.Action("LoginAs", "Users", new { userId });
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