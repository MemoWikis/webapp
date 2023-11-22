using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;
public class RedirectToErrorPage_IfNotLoggedInAttribute : Attribute{}
public class RedirectToErrorPage_IfNotLoggedInFilter : ActionFilterAttribute
{
    private readonly SessionUser _sessionUser;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RedirectToErrorPage_IfNotLoggedInFilter(SessionUser sessionUser,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor)
    {
        _sessionUser = sessionUser;
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!_sessionUser.IsLoggedIn)
            filterContext.Result = new RedirectResult(new Links(_actionContextAccessor, _httpContextAccessor).ErrorNotLoggedIn(filterContext.HttpContext.Request.Path));

        base.OnActionExecuting(filterContext);
    }
}