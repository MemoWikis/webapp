using Microsoft.AspNetCore.Mvc;

public class MiddlewareStartpageController(SessionUser _sessionUser) : Controller
{
    public readonly record struct TinyPage(int Id, string Name);

    [HttpGet]
    public TinyPage Get()
    {
        var topic = _sessionUser.IsLoggedIn
            ? EntityCache.GetPage(_sessionUser.User.StartPageId)
            : RootPage.Get;
        return new TinyPage { Name = topic.Name, Id = topic.Id };
    }
}