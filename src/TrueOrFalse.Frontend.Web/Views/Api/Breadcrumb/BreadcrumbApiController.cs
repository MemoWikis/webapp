﻿using System.Linq;
using System.Web.Mvc;

public class BreadCrumbApiController : BaseController
{
    [HttpPost]
    public JsonResult SetWiki(int wikiId, int currentCategoryId)
    {
        var defaultWikiId = SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : 1;
        SessionUser.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var category = EntityCache.GetCategory(currentCategoryId);
        var currentWiki = CrumbtrailService.GetWiki(category);
        SessionUser.SetWikiId(currentWiki);

        var model = new BaseModel();
        model.TopNavMenu.BreadCrumbCategories = CrumbtrailService.BuildCrumbtrail(category, currentWiki);
        var firstChevron = ViewRenderer.RenderPartialView("/Views/Categories/Detail/Partials/BreadCrumbFirstChevron.ascx", model, ControllerContext);
        var breadcrumbTrail = ViewRenderer.RenderPartialView("/Views/Categories/Detail/Partials/BreadCrumbTrail.ascx", model, ControllerContext);
        var breadcrumbHasGlobalWiki = model.TopNavMenu.BreadCrumbCategories.Items.Any(c => c.Category.Id == RootCategory.RootCategoryId);

        return Json(new
        {
            newWikiId = currentWiki.Id,
            firstChevron,
            breadcrumbTrail,
            breadcrumbHasGlobalWiki
        });
    }
}