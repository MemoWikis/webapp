﻿using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class TopicController : BaseController
{
    private readonly PermissionCheck _permissionCheck;

    public TopicController(SessionUser sessionUser,PermissionCheck permissionCheck) : base(sessionUser)
    {
        _permissionCheck = permissionCheck;
    }

    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        var topicControllerLogic = new TopicControllerLogic(_sessionUser,_permissionCheck);
        return Json(topicControllerLogic.GetTopicData(id), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetTopicWithSegments(int id)
    {
        var topicControllerLogic = new TopicControllerLogic(_sessionUser,_permissionCheck);
        return Json(topicControllerLogic.GetTopicDataWithSegments(id, ControllerContext), JsonRequestBehavior.AllowGet);
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
            var userCacheItem = SessionUserCache.GetItem(User_().Id);
            return Json(topicCacheItem
                .GetAggregatedQuestionsFromMemoryCache()
                .Where(q =>
                    q.Creator.Id == userCacheItem.Id &&
                    q.IsPrivate() &&
                    _permissionCheck.CanEdit(q))
                .Select(q => q.Id).ToList(), JsonRequestBehavior.AllowGet);
        }
        return Json(new { }, JsonRequestBehavior.DenyGet);
    }
}

