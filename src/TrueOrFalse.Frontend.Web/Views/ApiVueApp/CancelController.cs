using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Infrastructure;

namespace VueApp;

public class CancelController : BaseController
{
    [HttpGet]
    public JsonResult GetHelperTopics()
    {
        var count = RootCategory.MemuchoHelpIds.Count;
        var list = new List<object>();
        for (var i = 0; i < count; i++)
        {
            var category = EntityCache.GetCategory(RootCategory.MemuchoHelpIds[i]);

            list.Add(new
            {
                name = category.Name,
                link = Links.CategoryDetail(category)
            });
        }

        var standardFetch = new StandardFetchResult<List<object>>(list);
        return Json(standardFetch, JsonRequestBehavior.AllowGet);
    }
}