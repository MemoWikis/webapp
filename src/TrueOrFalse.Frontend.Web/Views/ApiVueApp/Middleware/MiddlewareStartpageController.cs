using Microsoft.AspNetCore.Mvc;

public class MiddlewareStartpageController(SessionUser _sessionUser) : Controller
{
    public readonly record struct TinyTopic(int Id, string Name);

    [HttpGet]
    public TinyTopic Get()
    {
        var topic = _sessionUser.IsLoggedIn
            ? EntityCache.GetCategory(_sessionUser.User.StartTopicId)
            : RootCategory.Get;
        return new TinyTopic { Name = topic.Name, Id = topic.Id };
    }
}