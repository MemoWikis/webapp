using System.Web.Mvc;

public class AccessOnlyAsLoggedInAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!SessionUserLegacy.IsLoggedIn)
            throw new InvalidAccessException();

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(SessionUserLegacy.UserId);

        base.OnActionExecuting(filterContext);
    }
}