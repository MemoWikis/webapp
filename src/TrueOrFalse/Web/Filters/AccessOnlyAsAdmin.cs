using System.Web.Mvc;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;


public class AccessOnlyAsAdmin : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var userSession = new SessionUser();
        if (!userSession.IsInstallationAdmin)
            throw new InvalidAccessException();

        base.OnActionExecuting(filterContext);
    }
}