using System.Web.Mvc;

public class AccessOnlyAsLoggedInAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var userSession = new SessionUser();
        if (!userSession.IsLoggedIn)
            throw new InvalidAccessException();

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(userSession.UserId);

        base.OnActionExecuting(filterContext);
    }
}