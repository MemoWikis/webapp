using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;


[SetUserMenu(UserMenuEntry.None)]
public class CategoryController : BaseController
{
    private const string _viewLocation = "~/Views/Categories/Detail/Category.aspx";
    private const string _topicTab = "~/Views/Categories/Detail/Tabs/TopicTab.ascx";

    [SetMainMenu(MainMenuEntry.CategoryDetail)]
    [SetThemeMenu(true)]
    public ActionResult Category(int id, int? version, bool? openEditMode)
    {
        GetMyWorldCookie(); 
        var modelAndCategoryResult = LoadModel(id, version, openEditMode);
        modelAndCategoryResult.CategoryModel.IsInTopic = true;
        return View(_viewLocation, modelAndCategoryResult.CategoryModel);
    }

    public ActionResult CategoryLearningTab(int id, int? version)
    {
        var modelAndCategoryResult = LoadModel(id, version);
        modelAndCategoryResult.CategoryModel.IsDisplayNoneSessionConfigNote = GetSettingsCookie("SessionConfigTopNote");
        modelAndCategoryResult.CategoryModel.IsDisplayNoneSessionConfigNoteQuestionList = !GetSettingsCookie("SessionConfigQuestionList");
        modelAndCategoryResult.CategoryModel.IsInLearningTab = true;

        return View(_viewLocation, modelAndCategoryResult.CategoryModel);
    }

    public ActionResult CategoryAnalyticsTab(int id, int? version)
    {
        var modelAndCategoryResult = LoadModel(id, version);
        modelAndCategoryResult.CategoryModel.IsInAnalyticsTab = true;

        return View(_viewLocation, modelAndCategoryResult.CategoryModel);
    }

    private LoadModelResult LoadModel(int id, int? version, bool? openEditMode = false)
    {
        var result = new LoadModelResult();
        Category category;
        var allCategories = EntityCache.GetAllCategories(); 
        category = EntityCache.GetCategory(id);
        
        var isCategoryNull = category == null;

        if (isCategoryNull)
        {
            category = new Category();
            category.Id = id;
            category.Name = "";
        }

        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category, HistoryItemType.Any, isCategoryNull));
        result.Category = category;

        result.CategoryModel =
            GetModelWithContentHtml(category, version, isCategoryNull, openEditMode ?? false);

        if (version != null)
            ApplyCategoryChangeToModel(result.CategoryModel, (int)version, id);
        else
            SaveCategoryView.Run(result.Category, User_());

        return result;
    }

    public ActionResult GetTopicTabAsync(int id)
    {
        return View(_topicTab, LoadModel(id, null).CategoryModel);
    }

    private void ApplyCategoryChangeToModel(CategoryModel categoryModel, int version, int id = -1)
    {
        var ListChangesById = Sl.CategoryChangeRepo.GetForCategory(id);
        var haveVersionData = ListChangesById.Where(lc => lc.Id == version).FirstOrDefault().Type != CategoryChangeType.Delete;
        var IsCategoryDeleted = ListChangesById.LastOrDefault().Type == CategoryChangeType.Delete;

        var categoryChange = Sl.CategoryChangeRepo.GetByIdEager(version);
        Sl.Session.Evict(categoryChange);

        categoryChange.Category = IsCategoryDeleted ? new Category() : categoryChange.Category;
        categoryChange.Category.Id = id;

        var historicCategory = categoryChange.ToHistoricCategory(haveVersionData);

        categoryModel.Name = historicCategory.Name;
        categoryModel.CategoryChange = categoryChange;
        categoryModel.CustomPageHtml = MarkdownToHtml.Run(historicCategory.TopicMarkdown, historicCategory, ControllerContext, version);
        categoryModel.Description = MarkdownToHtml.Run(historicCategory.Description, historicCategory, ControllerContext);
        categoryModel.WikipediaURL = historicCategory.WikipediaURL;
        categoryModel.NextRevExists = Sl.CategoryChangeRepo.GetNextRevision(categoryChange) != null;
    }

    private CategoryModel GetModelWithContentHtml(Category category, int? version = null, bool isCategoryNull = false, bool openEditMode = false)
    {
        var isQuestionListSessionConfigNoteDisplay = !GetSettingsCookie("SessionConfigQuestionList");

        return new CategoryModel(category, true, isCategoryNull, openEditMode)
        {
            IsDisplayNoneSessionConfigNoteQuestionList = isQuestionListSessionConfigNoteDisplay,
            CustomPageHtml = string.IsNullOrEmpty(category.Content) ? "" : TemplateToHtml.Run(Tokenizer.Run(category.Content), category, ControllerContext, version)
        };
    }
    public void CategoryById(int id)
    {
        Response.Redirect(Links.CategoryDetail(Resolve<CategoryRepository>().GetById(id)));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult Restore(int categoryId, int categoryChangeId)
    {
        RestoreCategory.Run(categoryChangeId, this.User_());

        var categoryName = Sl.CategoryRepo.GetById(categoryId).Name;
        return Redirect(Links.CategoryDetail(categoryName, categoryId));
    }

    public ActionResult StartTestSession(int categoryId)
    {
        var categoryName = EntityCache.GetCategory(categoryId).Name;

        return Redirect(Links.TestSession(categoryName, categoryId));
    }

    public ActionResult StartTestSessionForSetsInCategory(List<int> setIds, string setListTitle, int categoryId)
    {
        var sets = Sl.SetRepo.GetByIds(setIds);
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        var testSession = new TestSession(sets, setListTitle, category);

        Sl.SessionUser.AddTestSession(testSession);

        return Redirect(Links.TestSession(testSession.UriName, testSession.Id));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int categoryId)
    {
        var config = new LearningSessionConfig
        {
            CategoryId = categoryId
        };
        var learningSession = LearningSessionNewCreator.ForAnonymous(config);

        return Redirect(Links.LearningSession(learningSession));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSessionForSets(List<int> setIds, string setListTitle)
    {
        var sets = Sl.SetRepo.GetByIds(setIds);
        var questions = sets.SelectMany(s => s.Questions()).Distinct().ToList();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");
        var config = new LearningSessionConfig();
        var learningSession = LearningSessionNewCreator.ForAnonymous(config);

        return Redirect(Links.LearningSession(learningSession));
    }

   [HttpPost]
    public string Tab(string tabName, int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        var isCategoryNull = category == null;
        category = isCategoryNull ? new Category() : category;

        return ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/Tabs/" + tabName + ".ascx",
            GetModelWithContentHtml(category, null, isCategoryNull),
            ControllerContext
        );
    }
    [HttpGet]
    public string KnowledgeBar(int categoryId) =>
        ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/CategoryKnowledgeBar.ascx",
            new CategoryKnowledgeBarModel(Sl.CategoryRepo.GetById(categoryId)),
            ControllerContext
        );
    [HttpPost]
    public string WishKnowledgeInTheBox(int categoryId) =>
        ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/Partials/WishKnowledgeInTheBox.ascx",
            new WishKnowledgeInTheBoxModel(Sl.CategoryRepo.GetById(categoryId)),
            ControllerContext
        );

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public ActionResult SaveCategoryContent(int categoryId, List<TemplateParser.JsonLoader> content = null)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);

        if (category != null)
        { 
            if (content != null)
                category.Content = TemplateParser.GetContent(content);
            else category.Content = null;
                category.Content = TemplateParser.GetContent(content);

            Sl.CategoryRepo.Update(category, User_());

            return Json(true);
        }
        return Json(false);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public ActionResult RenderMarkdown(int categoryId, string markdown)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);

        return Json(MarkdownSingleTemplateToHtml.Run(markdown, category, this.ControllerContext, true));
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public ActionResult RenderContentModule(int categoryId, TemplateParser.JsonLoader json)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        var templateJson = new TemplateJson
        {
            TemplateName = json.TemplateName,
            OriginalJson = JsonConvert.SerializeObject(json, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            })
        };
        var baseContentModule = TemplateParserForSingleTemplate.Run(templateJson, category);
        var html = MarkdownSingleTemplateToHtml.GetHtml(
            baseContentModule,
            category,
            this.ControllerContext
        );
        return Json(html);
    }

    public string GetKnowledgeGraphDisplay(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        category = category == null ? new Category() : category;

        return ViewRenderer.RenderPartialView("~/Views/Categories/Detail/Partials/KnowledgeGraph/KnowledgeGraph.ascx", new KnowledgeGraphModel(category), ControllerContext);
    }

    public string RenderNewKnowledgeSummaryBar(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        return ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category), ControllerContext);
    }

    public void SetSettingsCookie(string name)
    {

        HttpCookie cookie = Request.Cookies.Get(name);

        // Check if cookie exists in the current request.
        if (cookie == null)
        {
            // Create cookie.
            cookie = new HttpCookie(name);
            // Set value of cookie to current date time.
            cookie.Value = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            // Set cookie to expire in 10 minutes.
            cookie.Expires = DateTime.Now.AddYears(1);
            // Insert the cookie in the current HttpResponse.
            Response.Cookies.Add(cookie);
        }
        else
        {
            Logg.r().Error("Cookie is available");
        }
    }

    public bool GetSettingsCookie(string name)
    {
        HttpCookie cookie = Request.Cookies.Get(name);
        if (cookie != null)
            return true;

        return false;
    }

    public void SetMyWorldCookie(bool showMyWorld)
    {
        var myWorldCookieName = "memucho_myworld";
        HttpCookie cookie = Request.Cookies.Get(myWorldCookieName);

        if (cookie == null)
        {
            cookie = new HttpCookie(myWorldCookieName);
            cookie.Value = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            cookie.Expires = DateTime.Now.AddYears(1);
            cookie.Values.Add("showMyWorld", showMyWorld.ToString());
        }
        else
        {
            cookie.Values["showMyWorld"] = showMyWorld.ToString();
        }

        if (_sessionUser.IsLoggedIn && _sessionUser.User.Name != "User")
        {
            if (!UserEntityCache.IsCategoryCacheKeyAvailable())
                UserEntityCache.Init();
        }
        else if(_sessionUser.IsLoggedIn)
        {
            UserEntityCache.Init(true);
        }

        UserCache.IsFiltered = showMyWorld;
        Response.Cookies.Add(cookie);
    }
    public bool GetMyWorldCookie()
    {
        HttpCookie cookie = Request.Cookies.Get("memucho_myworld");
        if (cookie != null && IsLoggedIn)
        {
            var val = cookie.Values["showMyWorld"];
            if (val == "True")
            {
                UserCache.IsFiltered = true;
                return true;
            }
        }

        if (!IsLoggedIn)
        {
            SetMyWorldCookie(false);
        }

        UserCache.IsFiltered = false; 
        return false;
    }

    public bool DeleteCookie()
    {
        var myWorldCookieName = "memucho_myworld";
        HttpCookie cookie = Request.Cookies.Get(myWorldCookieName);

        if (cookie != null)
        {
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }
        return true;
    }
}
public class LoadModelResult
{
    public Category Category;
    public CategoryModel CategoryModel;
}