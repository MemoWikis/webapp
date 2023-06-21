using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class MiddlewareStartpageController : BaseController
{
    [HttpGet]
    public JsonResult Get()
    {
        var topic = SessionUser.IsLoggedIn ? EntityCache.GetCategory(SessionUser.User.StartTopicId) : RootCategory.Get;
        return Json(new { name = topic.Name, id = topic.Id }, JsonRequestBehavior.AllowGet);
    }
}   