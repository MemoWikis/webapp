using Microsoft.AspNetCore.Mvc;
using VueApp;

public class TopicController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly TopicControllerLogic _topicControllerLogic;
    private readonly SessionUserCache _sessionUserCache;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly PersistentLoginRepo _persistentLoginRepo;

    public TopicController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        TopicControllerLogic topicControllerLogic,
        SessionUserCache sessionUserCache,
        UserReadingRepo userReadingRepo,
        PersistentLoginRepo persistentLoginRepo) : base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _topicControllerLogic = topicControllerLogic;
        _sessionUserCache = sessionUserCache;
        _userReadingRepo = userReadingRepo;
        _persistentLoginRepo = persistentLoginRepo;
    }

    [HttpGet]
    public JsonResult GetTopic([FromRoute] int id)
    {
        return Json(_topicControllerLogic.GetTopicData(id));
    }
}

