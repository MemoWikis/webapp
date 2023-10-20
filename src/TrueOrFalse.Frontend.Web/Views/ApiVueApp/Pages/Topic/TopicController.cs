using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueApp;

public class TopicController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly TopicControllerLogic _topicControllerLogic;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly PersistentLoginRepo _persistentLoginRepo;

    public TopicController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        TopicControllerLogic topicControllerLogic,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        UserReadingRepo userReadingRepo,
        PersistentLoginRepo persistentLoginRepo)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _topicControllerLogic = topicControllerLogic;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
        _userReadingRepo = userReadingRepo;
        _persistentLoginRepo = persistentLoginRepo;
    }

    public JsonResult GetTopic([FromRoute] int id)
    {
        var topic = Json(_topicControllerLogic.GetTopicData(id));
        return topic; 
    }

    public bool CanAccess([FromQuery] int id)
    {
        var c = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(c))
            return true;

        return false;
    }

    public JsonResult LoadQuestionIds([FromQuery] int topicId)
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

