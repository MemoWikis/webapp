using System.Web.Mvc;

namespace VueApp;

public class TopicLoaderController : BaseController
{
    [HttpGet]
    public JsonResult GetTopic(int? id)
    {
        id ??= SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : 1;

        var topicControllerLogic = new TopicControllerLogic();
        return Json(topicControllerLogic.GetTopicData((int)id), JsonRequestBehavior.AllowGet);
    }
}   