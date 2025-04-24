using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace VueApp;

public class CancelController : Controller
{
    public readonly record struct TinyPage(string Name, int Id);
    [HttpGet]
    public List<TinyPage> GetHelperPages()
    {
        var count = FeaturedPage.MemoWikisHelpIds.Count;
        var list = new List<TinyPage>();
        for (var i = 0; i < count; i++)
        {
            var page = EntityCache.GetPage(FeaturedPage.MemoWikisHelpIds[i]);

            list.Add(new TinyPage
            (
                Name: page.Name,
                Id: page.Id
            ));
        }
        return list;
    }
}