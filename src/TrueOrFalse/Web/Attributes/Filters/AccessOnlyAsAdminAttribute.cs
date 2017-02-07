using System.Web.Mvc;

public class AccessOnlyAsAdminAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var userSession = new SessionUser();
        if (!userSession.IsInstallationAdmin)
            throw new InvalidAccessException();

        base.OnActionExecuting(filterContext);
    }
}