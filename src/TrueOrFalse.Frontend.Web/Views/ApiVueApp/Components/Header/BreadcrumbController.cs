using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain;

namespace VueApp;
public class BreadcrumbController(SessionUser _sessionUser, BreadcrumbCreator _breadcrumbCreator) : Controller
{
    [HttpPost]
    public Breadcrumb GetBreadcrumb([FromBody] BreadcrumbCreator.GetBreadcrumbParam param)
    {
        return _breadcrumbCreator.GetBreadcrumb(param); 
    }

    [HttpGet]
    public BreadcrumbItem GetPersonalWiki()
    {
        var topic = _sessionUser.IsLoggedIn ? EntityCache.GetCategory(_sessionUser.User.StartTopicId) : RootCategory.Get;
        return new BreadcrumbItem
        {
            Name = topic.Name,
            Id = topic.Id
        };
    }
}