using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentNHibernate.Data;
using TrueOrFalse.Frontend.Web.Code;


[SetUserMenu(UserMenuEntry.None)]
public class CategoryController : BaseController
{
    private const string _viewLocation = "~/Views/Categories/Detail/Category.aspx";
    private const string _topicTab = "~/Views/Categories/Detail/Tabs/TopicTab.ascx";

    [SetMainMenu(MainMenuEntry.CategoryDetail)]
    [SetThemeMenu(true)]
    public ActionResult Category(int id, int? version)
    {
        GetMyWorldCookie(); 
        var modelAndCategoryResult = LoadModel(id, version);
        modelAndCategoryResult.CategoryModel.IsInTopic = true;
        return View(_viewLocation, modelAndCategoryResult.CategoryModel);
    }

    public ActionResult CategoryLearningTab(int id, int? version)
    {
        GetMyWorldCookie();

        var modelAndCategoryResult = LoadModel(id, version);
        modelAndCategoryResult.CategoryModel.ShowLearningSessionConfigurationMessageForTab = GetSettingsCookie("ShowSessionConfigurationMessageTab");
        modelAndCategoryResult.CategoryModel.ShowLearningSessionConfigurationMessageForQuestionList = !GetSettingsCookie("SessionConfigurationMessageList");
        modelAndCategoryResult.CategoryModel.IsInLearningTab = true;

        return View(_viewLocation, modelAndCategoryResult.CategoryModel);
    }

    public ActionResult CategoryAnalyticsTab(int id, int? version)
    {
        var modelAndCategoryResult = LoadModel(id, version);
        modelAndCategoryResult.CategoryModel.IsInAnalyticsTab = true;

        return View(_viewLocation, modelAndCategoryResult.CategoryModel);
    }

    private LoadModelResult LoadModel(int id, int? version)
    {
        var result = new LoadModelResult();
        var category = EntityCache.GetCategoryCacheItem(id);
        
        var isCategoryNull = category == null;

        if (isCategoryNull)
        {
            category = new CategoryCacheItem();
            category.Id = id;
            category.Name = "";
        }

        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category, HistoryItemType.Any, isCategoryNull));
        result.Category = category;

        result.CategoryModel =
            GetModelWithContentHtml(category, version, isCategoryNull);

        if (version != null)
            ApplyCategoryChangeToModel(result.CategoryModel, (int)version, id);
        else
            SaveCategoryView.Run(EntityCache.GetCategory(result.Category.Id), User_());

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
        categoryModel.CustomPageHtml = TemplateToHtml.Run( CategoryCacheItem.ToCacheCategory(historicCategory), ControllerContext);
        categoryModel.WikipediaURL = historicCategory.WikipediaURL;
        categoryModel.NextRevExists = Sl.CategoryChangeRepo.GetNextRevision(categoryChange) != null;
    }

    private CategoryModel GetModelWithContentHtml(CategoryCacheItem category, int? version = null, bool isCategoryNull = false)
    {
        return new CategoryModel(category, true, isCategoryNull)
        {
            ShowLearningSessionConfigurationMessageForQuestionList = !GetSettingsCookie("SessionConfigurationMessageList"),
            CustomPageHtml = TemplateToHtml.Run(category, ControllerContext)
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
        var categoryName = EntityCache.GetCategoryCacheItem(categoryId).Name;

        return Redirect(Links.TestSession(categoryName, categoryId));
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int categoryId)
    {
        var config = new LearningSessionConfig
        {
            CategoryId = categoryId
        };
        var learningSession = LearningSessionCreator.ForAnonymous(config);

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
        var learningSession = LearningSessionCreator.ForAnonymous(config);

        return Redirect(Links.LearningSession(learningSession));
    }

   [HttpPost]
    public string Tab(string tabName, int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(categoryId);
        var isCategoryNull = categoryCacheItem == null;
        categoryCacheItem = isCategoryNull ? new CategoryCacheItem() : categoryCacheItem;

        return ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/Tabs/" + tabName + ".ascx",
            GetModelWithContentHtml(categoryCacheItem, null, isCategoryNull),
            ControllerContext
        );
    }
    [HttpGet]
    public string KnowledgeBar(int categoryId) =>
        ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/CategoryKnowledgeBar.ascx",
            new CategoryKnowledgeBarModel(EntityCache.GetCategoryCacheItem(categoryId)),
            ControllerContext
        );
    [HttpPost]
    public string WishKnowledgeInTheBox(int categoryId) =>
        ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/Partials/WishKnowledgeInTheBox.ascx",
            new WishKnowledgeInTheBoxModel(Sl.CategoryRepo.GetById(categoryId)),
            ControllerContext
        );

    public string GetKnowledgeGraphDisplay(int categoryId)
    {
        var category = EntityCache.GetCategoryCacheItem(categoryId);
        category = category == null ? new CategoryCacheItem() : category;

        return ViewRenderer.RenderPartialView("~/Views/Categories/Detail/Partials/KnowledgeGraph/KnowledgeGraph.ascx", new KnowledgeGraphModel(category), ControllerContext);
    }

    public string RenderNewKnowledgeSummaryBar(int categoryId)
    {
        var category = EntityCache.GetCategoryCacheItem(categoryId);
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
        HttpCookie cookie = new HttpCookie(myWorldCookieName);
        cookie.Expires = DateTime.Now.AddYears(1);
        cookie.Values.Add("showMyWorld", showMyWorld.ToString());

        if (_sessionUser.IsLoggedIn)
        {
            if (!UserEntityCache.IsCategoryCacheKeyAvailable())
                UserEntityCache.Init();
        }

        UserCache.GetItem(_sessionUser.UserId).IsFiltered = showMyWorld;
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
                UserCache.GetItem(_sessionUser.UserId).IsFiltered = true;
                return true;
            }
        }

        if (!IsLoggedIn)
        {
            SetMyWorldCookie(false);
        }

        UserCache.GetItem(_sessionUser.UserId).IsFiltered = false; 
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

        UserCache.GetItem(_sessionUser.UserId).IsFiltered = false; 
        return true;
    }

    [HttpPost]
    public bool GetWishknowledge(int categoryId)
    {
        if (!IsLoggedIn)
            return false;

        var userValuation = UserCache.GetItem(UserId).CategoryValuations;
        var isInWishknowledge = false;
        if (userValuation.ContainsKey(categoryId))
            isInWishknowledge = userValuation[categoryId].IsInWishKnowledge();

        return isInWishknowledge;
    }
    
    [HttpPost]
    public bool AddToWishknowledge(int categoryId)
    {
        var user = User_();
        var userValuation = UserCache.GetItem(user.Id).CategoryValuations;
        var isInWishknowledge = false;
        if (userValuation.ContainsKey(categoryId) && user != null) { }
        isInWishknowledge = userValuation[categoryId].IsInWishKnowledge();

        return isInWishknowledge;
    }
}
public class LoadModelResult
{
    public CategoryCacheItem Category;
    public CategoryModel CategoryModel;
}