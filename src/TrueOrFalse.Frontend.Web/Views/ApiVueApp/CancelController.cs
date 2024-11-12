using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace VueApp;

public class CancelController(IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    public readonly record struct TinyPage(string Name, int Id);
    [HttpGet]
    public List<TinyPage> GetHelperPages()
    {
        var count = RootPage.MemuchoHelpIds.Count;
        var list = new List<TinyPage>();
        for (var i = 0; i < count; i++)
        {
            var page = EntityCache.GetPage(RootPage.MemuchoHelpIds[i]);

            list.Add(new TinyPage
            (
                Name: page.Name,
                Id: page.Id
            ));
        }
        return list;
    }
}