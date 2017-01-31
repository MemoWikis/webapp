using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class RedirectToErrorPage_IfNotLoggedInAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var userSession = new SessionUser();
        if (!userSession.IsLoggedIn)
            filterContext.Result = new RedirectResult(Links.ErrorNotLoggedIn(filterContext.HttpContext.Request.Path));

        base.OnActionExecuting(filterContext);
    }
}