using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class MiddlewareStartpageController : BaseController
{
    [HttpGet]
    public JsonResult Get()
    {
        var topic = SessionUserLegacy.IsLoggedIn ? EntityCache.GetCategory(SessionUserLegacy.User.StartTopicId) : RootCategory.Get;
        return Json(new { encodedName = UriSanitizer.Run(topic.Name), id = topic.Id }, JsonRequestBehavior.AllowGet);
    }
}   