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

    [HttpGet]
    public JsonResult GetTopic([FromRoute] int id)
    {
        return Json(_topicControllerLogic.GetTopicData(id));
    }
}

