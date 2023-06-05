using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class TopicController : BaseController
{
    private readonly SessionUser _sessionUser;

    public TopicController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        var topicControllerLogic = new TopicControllerLogic(_sessionUser);
        return Json(topicControllerLogic.GetTopicData(id), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetTopicWithSegments(int id)
    {
        var topicControllerLogic = new TopicControllerLogic(_sessionUser);
        return Json(topicControllerLogic.GetTopicDataWithSegments(id, ControllerContext), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public bool CanAccess(int id)
    {
        var c = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(c))
            return true;
            
        return false;
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

