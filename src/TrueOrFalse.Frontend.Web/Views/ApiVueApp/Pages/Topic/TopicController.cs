using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class TopicController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryViewRepo _categoryViewRepo;

    public TopicController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryValuationRepo categoryValuationRepo,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryViewRepo categoryViewRepo) : base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _categoryValuationRepo = categoryValuationRepo;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryViewRepo = categoryViewRepo;
    }

    [HttpGet]
    public JsonResult GetTopic(int id)
    {
        var topicControllerLogic = new TopicControllerLogic(_sessionUser, _permissionCheck, _knowledgeSummaryLoader,
            _categoryValuationRepo, _categoryViewRepo);
        return Json(topicControllerLogic.GetTopicData(id), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetTopicWithSegments(int id)
    {
        var topicControllerLogic = new TopicControllerLogic(_sessionUser,_permissionCheck, _knowledgeSummaryLoader,
            _categoryValuationRepo,_categoryViewRepo );
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
            var userCacheItem = SessionUserCache.GetItem(User_().Id, _categoryValuationRepo);
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

