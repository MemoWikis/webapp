﻿

using Microsoft.AspNetCore.Mvc;

namespace VueApp;

[Route("apiVue/MiddlewareStartpage")]
[ApiController]
public class MiddlewareStartpageController : BaseController
{
    public MiddlewareStartpageController(SessionUser sessionUser) :base(sessionUser)
    {
        
    }

    [HttpGet]
    [Route("Get")]
    public JsonResult Get()
    {
        var topic = _sessionUser.IsLoggedIn ? EntityCache.GetCategory(_sessionUser.User.StartTopicId) : RootCategory.Get;
        return Json(new { name = topic.Name, id = topic.Id });
    }
}   