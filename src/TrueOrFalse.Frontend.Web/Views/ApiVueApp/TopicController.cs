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
}

public class TopicModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImgUrl { get; set; }
    public string Content { get; set; }
}