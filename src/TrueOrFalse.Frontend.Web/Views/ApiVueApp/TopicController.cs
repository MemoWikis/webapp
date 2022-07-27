using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class TopicController : BaseController
{
    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        var category = EntityCache.GetCategory(id);
        return Json(new TopicModel
        {
            Id = id,
            Name = category.Name,
            ImgUrl = new CategoryImageSettings(id).GetUrl_128px(asSquare: true).Url,
            Content = category.Content,
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public bool CanAccess(int id)
    {
        var c = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(c))
            return true;

        return false;
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveTopic(int id, string name, bool saveName, string content, bool saveContent)
    {
        if (!PermissionCheck.CanEditCategory(id))
            return Json("Dir fehlen leider die Rechte um die Seite zu bearbeiten");

        var categoryCacheItem = EntityCache.GetCategory(id);
        var category = Sl.CategoryRepo.GetById(categoryCacheItem.Id);

        if (categoryCacheItem == null || category == null)
            return Json(false);

        if (saveName)
        {
            categoryCacheItem.Name = name;
            category.Name = name;
        }

        if (saveContent)
        {
            categoryCacheItem.Content = content;
            category.Content = content;
        }
        EntityCache.AddOrUpdate(categoryCacheItem);
        Sl.CategoryRepo.Update(category, SessionUser.User, type: CategoryChangeType.Text);

        return Json(true);
    }
}

public class TopicModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImgUrl { get; set; }
    public string Content { get; set; }
    public string ChildCategoryIds { get; set; }
    public string SegmentJson { get; set; }
}