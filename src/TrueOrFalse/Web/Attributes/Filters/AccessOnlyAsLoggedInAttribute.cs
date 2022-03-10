using System.Web.Mvc;

public class AccessOnlyAsLoggedInAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!SessionUser.IsLoggedIn)
            throw new InvalidAccessException();

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(SessionUser.UserId);

        base.OnActionExecuting(filterContext);
    }
}