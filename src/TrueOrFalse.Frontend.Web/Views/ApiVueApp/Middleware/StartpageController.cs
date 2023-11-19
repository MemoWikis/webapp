using Microsoft.AspNetCore.Mvc;


public class MiddlewareStartpageController : BaseController
{
    public MiddlewareStartpageController(SessionUser sessionUser) : base(sessionUser)
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