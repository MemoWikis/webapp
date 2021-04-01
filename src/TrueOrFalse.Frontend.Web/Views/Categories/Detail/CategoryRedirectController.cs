using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

[SetUserMenu(UserMenuEntry.None)]
public class CategoryRedirectController : BaseController
{
    private const string _viewLocation = "~/Views/Categories/Detail/Category.aspx";

    [SetMainMenu(MainMenuEntry.CategoryDetail)]
    [SetThemeMenu(true)]
    public ActionResult Category(string text, int id, int? version)
    {
        if (SeoUtils.HasUnderscores(text))
            return SeoUtils.RedirectToHyphendVersion_Category(RedirectPermanent, text, id);

        return SeoUtils.RedirectToNewCategory(RedirectPermanent, text, id);
    }

    public ActionResult CategoryLearning(int id)
    {
        var modelAndCategoryResult = LoadModel(id);
        modelAndCategoryResult.CategoryModel.IsLearningTab = true;
        return View(_viewLocation, modelAndCategoryResult.CategoryModel);
    }

    private LoadModelResult LoadModel(int id)
    {
        var result = new LoadModelResult();

        var category = EntityCache.GetCategoryCacheItem(id);
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category));
        result.Category = category;
        result.CategoryModel = GetModelWithContentHtml(category);

       return result;
    }

    private ActionResult Category(CategoryCacheItem category, int? version)
    {
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category));

        var categoryModel = GetModelWithContentHtml(category);

        if (version != null)
            ApplyCategoryChangeToModel(categoryModel, (int)version);
        else
            SaveCategoryView.Run(EntityCache.GetCategoryCacheItem(category.Id), User_());
   
        return View(_viewLocation, categoryModel);
    }
    
    private CategoryModel GetModelWithContentHtml(CategoryCacheItem category)
    {
        return new CategoryModel(category)
        {
            CustomPageHtml = TemplateToHtml.Run(category, ControllerContext)
        };
    }
    
    private void ApplyCategoryChangeToModel(CategoryModel categoryModel, int version)
    {
        var categoryChange = Sl.CategoryChangeRepo.GetByIdEager(version);
        Sl.Session.Evict(categoryChange);
        var historicCategory = CategoryCacheItem.ToCacheCategory(categoryChange.ToHistoricCategory());
        categoryModel.Name = historicCategory.Name;
        categoryModel.CategoryChange = categoryChange;
        categoryModel.CustomPageHtml = TemplateToHtml.Run(historicCategory, ControllerContext); 
        categoryModel.WikipediaURL = historicCategory.WikipediaURL;
        categoryModel.NextRevExists = Sl.CategoryChangeRepo.GetNextRevision(categoryChange) != null;
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

    public string Tab(string tabName, int categoryId)
    {
        return ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/Tabs/" + tabName + ".ascx",
            GetModelWithContentHtml(EntityCache.GetCategoryCacheItem(categoryId)),
            ControllerContext
        );
    }

    public string KnowledgeBar(int categoryId) => 
        ViewRenderer.RenderPartialView(
            "/Views/Categories/Detail/CategoryKnowledgeBar.ascx",
            new CategoryKnowledgeBarModel(EntityCache.GetCategoryCacheItem(categoryId)), 
            ControllerContext
        );

  
    public string WishKnowledgeInTheBox(int categoryId) =>
        ViewRenderer.RenderPartialView( 
            "/Views/Categories/Detail/Partials/WishKnowledgeInTheBox.ascx",
            new WishKnowledgeInTheBoxModel(Sl.CategoryRepo.GetById(categoryId)),
            ControllerContext
        );

}

