using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq.Expressions;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Frontend.Web.Code;

public class BreadCrumbApiController : BaseController
{
    [HttpPost]
    public JsonResult SetWiki(int wikiId, int currentCategoryId)
    {
        var defaultWikiId = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1;
        _sessionUser.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var category = EntityCache.GetCategoryCacheItem(currentCategoryId);
        var currentWiki = CrumbtrailService.GetWiki(category);
        _sessionUser.SetWikiId(currentWiki);

        var model = new BaseModel();
        model.TopNavMenu.BreadCrumbCategories = CrumbtrailService.BuildCrumbtrail(category, currentWiki);
        var firstChevron = ViewRenderer.RenderPartialView("/Views/Categories/Detail/Partials/BreadCrumbFirstChevron.ascx", model, ControllerContext);
        var breadcrumbTrail = ViewRenderer.RenderPartialView("/Views/Categories/Detail/Partials/BreadCrumbTrail.ascx", model, ControllerContext);

        return Json(new
        {
            newWikiId = currentWiki.Id,
            firstChevron,
            breadcrumbTrail,
        });
    }
}