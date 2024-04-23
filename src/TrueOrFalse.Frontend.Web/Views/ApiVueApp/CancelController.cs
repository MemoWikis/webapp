using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class CancelController(IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    public readonly record struct TinyTopic(string Name, string Link);
    [HttpGet]
    public List<TinyTopic> GetHelperTopics()
    {
        var count = RootCategory.MemuchoHelpIds.Count;
        var list = new List<TinyTopic>();
        for (var i = 0; i < count; i++)
        {
            var category = EntityCache.GetCategory(RootCategory.MemuchoHelpIds[i]);

            list.Add(new TinyTopic
            (
                Name: category.Name,
                Link: new Links(actionContextAccessor, httpContextAccessor).CategoryDetail(category)
            ));
        }
        return list;
    }
}