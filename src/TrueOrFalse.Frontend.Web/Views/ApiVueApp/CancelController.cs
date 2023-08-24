using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Infrastructure;

namespace VueApp;

public class CancelController : Controller
{
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CancelController(IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
    {
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
    }

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
                link = new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(category)
            });
        }

        var standardFetch = new StandardFetchResult<List<object>>(list);
        return Json(standardFetch);
    }
}