using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VueApp;


public class AppController : Controller
{
    private readonly VueSessionUser _vueSessionUser;

    public AppController(VueSessionUser vueSessionUser)
    {
        _vueSessionUser = vueSessionUser;
    }

    [HttpGet]
    public JsonResult GetCurrentUser()
    {
        return Json(_vueSessionUser.GetCurrentUserData());
    }

    [HttpGet]
    public JsonResult GetFooterTopics()
    {
        var json = new
        {
            RootWiki = new
            {
                Id = RootCategory.RootCategoryId,
                Name = EntityCache.GetCategory(RootCategory.RootCategoryId)?.Name
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
            Documentation = new
            {
                Id = RootCategory.IntroCategoryId,
                Name = EntityCache.GetCategory(RootCategory.IntroCategoryId).Name
            }
        }; 
        return Json(json);
    }
}