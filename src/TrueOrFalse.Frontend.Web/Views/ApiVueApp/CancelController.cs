using System.Collections.Generic;
using Antlr.Runtime;
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
               Link: new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(category)
            ));
        }
        return list;
    }
}