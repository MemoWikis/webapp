using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class TopicController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly TopicControllerLogic _topicControllerLogic;
    private readonly SessionUserCache _sessionUserCache;

    public TopicController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        TopicControllerLogic topicControllerLogic,
        SessionUserCache sessionUserCache) 
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _topicControllerLogic = topicControllerLogic;
        _sessionUserCache = sessionUserCache;
    }

    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        
        return Json(_topicControllerLogic.GetTopicData(id));
    }

    [HttpGet]
    public JsonResult GetTopicWithSegments(int id)
    {
            return Json(_topicControllerLogic.GetTopicDataWithSegments(id, ControllerContext));
        }

    [HttpGet]
    public bool CanAccess(int id)
    {
        var c = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(c))
            return true;

        return false;
    }

    [HttpGet]
    public JsonResult LoadQuestionIds(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);
        if (_permissionCheck.CanView(topicCacheItem))
        {
            var userCacheItem = _sessionUserCache.GetItem(_sessionUser.UserId);
            return Json(topicCacheItem
                .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId)
                .Where(q =>
                    q.Creator.Id == userCacheItem.Id &&
                    q.IsPrivate() &&
                    _permissionCheck.CanEdit(q))
                .Select(q => q.Id).ToList());
        }
        return Json(new { });
    }
}

