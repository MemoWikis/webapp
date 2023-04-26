using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class TopicController : BaseController
{
    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        var topicControllerLogik = new TopicControllerLogic();
        return Json(topicControllerLogik.GetTopicData(id), JsonRequestBehavior.AllowGet);
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
        var topicControllerLogic = new TopicControllerLogic();
        return Json(topicControllerLogic.SaveTopic(id, name, saveName, content, saveContent));
    }

    [HttpGet]
    public JsonResult LoadQuestionIds(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);
        if (PermissionCheck.CanView(topicCacheItem))
        {
            var userCacheItem = SessionUserCache.GetItem(User_().Id);
            return Json(topicCacheItem
                .GetAggregatedQuestionsFromMemoryCache()
                .Where(q =>
                    q.Creator.Id == userCacheItem.Id &&
                    q.IsPrivate() &&
                    PermissionCheck.CanEdit(q))
                .Select(q => q.Id).ToList(), JsonRequestBehavior.AllowGet);
        }
        return Json(new { }, JsonRequestBehavior.DenyGet);
    }
}

