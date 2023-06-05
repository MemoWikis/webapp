using System.Web.Mvc;

public class AccessOnlyAsAdminAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!SessionUserLegacy.IsInstallationAdmin)
            throw new InvalidAccessException();

        base.OnActionExecuting(filterContext);
    }
}