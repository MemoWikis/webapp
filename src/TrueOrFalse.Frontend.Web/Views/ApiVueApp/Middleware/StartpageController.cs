using Microsoft.AspNetCore.Mvc;


public class MiddlewareStartpageController : Controller
{
    private readonly SessionUser _sessionUser;

    public MiddlewareStartpageController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    [HttpGet]
    public JsonResult Get()
    {
        var topic = _sessionUser.IsLoggedIn ? EntityCache.GetCategory(_sessionUser.User.StartTopicId) : RootCategory.Get;
        return Json(new { name = topic.Name, id = topic.Id });
    }
}   