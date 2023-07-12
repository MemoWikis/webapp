using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class TopicController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly UserRepo _userRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly TopicControllerLogic _topicControllerLogic;

    public TopicController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryValuationRepo categoryValuationRepo,
        UserRepo userRepo,
        QuestionValuationRepo questionValuationRepo,
        TopicControllerLogic topicControllerLogic) 
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _categoryValuationRepo = categoryValuationRepo;
        _userRepo = userRepo;
        _questionValuationRepo = questionValuationRepo;
        _topicControllerLogic = topicControllerLogic;
    }

    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        
        return Json(_topicControllerLogic.GetTopicData(id), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetTopicWithSegments(int id)
    {
        return Json(_topicControllerLogic.GetTopicDataWithSegments(id, ControllerContext), JsonRequestBehavior.AllowGet);
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
            var userCacheItem = SessionUserCache.GetItem(_sessionUser.UserId, _categoryValuationRepo, _userRepo, _questionValuationRepo);
            return Json(topicCacheItem
                .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId)
                .Where(q =>
                    q.Creator.Id == userCacheItem.Id &&
                    q.IsPrivate() &&
                    _permissionCheck.CanEdit(q))
                .Select(q => q.Id).ToList(), JsonRequestBehavior.AllowGet);
        }
        return Json(new { }, JsonRequestBehavior.DenyGet);
    }
}

