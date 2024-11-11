using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class CancelController(IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    public readonly record struct TinyPage(string Name, string Link);
    [HttpGet]
    public List<TinyPage> GetHelperPages()
    {
        var count = RootPage.MemuchoHelpIds.Count;
        var list = new List<TinyPage>();
        for (var i = 0; i < count; i++)
        {
            var category = EntityCache.GetPage(RootPage.MemuchoHelpIds[i]);

            list.Add(new TinyPage
            (
                Name: category.Name,
                Link: new Links(actionContextAccessor, httpContextAccessor).CategoryDetail(category)
            ));
        }
        return list;
    }
}