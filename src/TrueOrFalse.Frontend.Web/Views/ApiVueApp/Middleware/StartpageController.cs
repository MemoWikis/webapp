using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class MiddlewareStartpageController : BaseController
{
    public MiddlewareStartpageController(SessionUser sessionUser) :base(sessionUser)
    {
        
    }

    [HttpGet]
    public JsonResult Get()
    {
        var topic = _sessionUser.IsLoggedIn ? EntityCache.GetCategory(_sessionUser.User.StartTopicId) : RootCategory.Get;
        return Json(new { name = topic.Name, id = topic.Id }, JsonRequestBehavior.AllowGet);
    }
}   