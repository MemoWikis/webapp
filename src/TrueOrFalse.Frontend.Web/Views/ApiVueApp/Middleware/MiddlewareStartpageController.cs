using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

public class MiddlewareStartpageController(SessionUser _sessionUser) : Controller
{
    public readonly record struct GetResult(int? Id, [CanBeNull] string Name, bool IsLoggedIn = false);

    [HttpGet]
    public GetResult Get()
    {
        if (_sessionUser.IsLoggedIn)
        {
            var page = EntityCache.GetPage(_sessionUser.User.StartPageId);
            return new GetResult(page.Id, page.Name, true);
        }

        return new GetResult();
    }
}