using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class FooterController : BaseController
{


    [HttpGet]
    public JsonResult GetRootWiki()
    {

        var rootTopic = EntityCache.GetCategory(RootCategory.RootCategoryId);

        return Json(new TopicModel
        {
            Id = rootTopic.Id,
            Name = rootTopic.Name
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetMainTopics()
    {
        var mainTopic = new List<TopicModel>();

        foreach (var id in RootCategory.MainCategoryIds)
            mainTopic.Add(new TopicModel
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            });

        return Json(mainTopic.ToArray(), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetMemoWiki()
    {

        var memoWiki = EntityCache.GetCategory(RootCategory.MemuchoWikiId);

        return Json(new TopicModel
        {
            Id = memoWiki.Id,
            Name = memoWiki.Name
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetMemoTopics()
    {
        var memoTopics = new List<TopicModel>();

        foreach (var id in RootCategory.MemuchoCategoryIds)
            memoTopics.Add(new TopicModel
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            });

        return Json(memoTopics.ToArray(), JsonRequestBehavior.AllowGet);
    }


    [HttpGet]
    public JsonResult GetHelpTopics()
    {
        var helpTopics = new List<TopicModel>();

        foreach (var id in RootCategory.MemuchoHelpIds)
            helpTopics.Add(new TopicModel
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            });

        return Json(helpTopics.ToArray(), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetPopularTopics()
    {
        var popularTopics = new List<TopicModel>();

        foreach (var id in RootCategory.PopularCategoryIds)
            popularTopics.Add(new TopicModel
            {
                Id = id,
                Name = EntityCache.GetCategory(id).Name
            });

        return Json(popularTopics.ToArray(), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public bool CanAccess(int id)
    {
        var c = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(c))
            return true;

        return false;
    }
}