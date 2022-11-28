using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class FooterController : BaseController
{
    [HttpGet]
    public JsonResult GetFooterTopics()
    {
        return Json(new
        {
            RootWiki = new
            {
                Id = RootCategory.RootCategoryId,
                Name = EntityCache.GetCategory(RootCategory.RootCategoryId).Name
            },
            MainTopics = RootCategory.MainCategoryIds.Select(id => new
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            }).ToArray(),
            MemoWiki = new
            {
                Id = RootCategory.MemuchoWikiId,
                Name = EntityCache.GetCategory(RootCategory.MemuchoWikiId).Name
            },
            MemoTopics = RootCategory.MemuchoCategoryIds.Select(id => new
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            }).ToArray(),
            HelpTopics = RootCategory.MemuchoHelpIds.Select(id => new
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            }).ToArray(),
            PopularTopics = RootCategory.PopularCategoryIds.Select(id => new
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            }).ToArray(),
        }, JsonRequestBehavior.AllowGet);
    }
}