using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrueOrFalse.Frontend.Web.Code;
public class RedirectToErrorPage_IfNotLoggedInAttribute : Attribute{}
public class RedirectToErrorPage_IfNotLoggedInFilter : ActionFilterAttribute
{
    private readonly SessionUser _sessionUser;

    public RedirectToErrorPage_IfNotLoggedInFilter(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!_sessionUser.IsLoggedIn)
            filterContext.Result = new RedirectResult(Links.ErrorNotLoggedIn(filterContext.HttpContext.Request.Path));

        base.OnActionExecuting(filterContext);
    }
}