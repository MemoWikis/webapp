using System.Web.Mvc;

public class AccessOnlyAsLoggedIn : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var userSession = new SessionUser();
        if (!userSession.IsLoggedIn)
            throw new InvalidAccessException();

        ThrowIfNot_IsUserOrAdmin.Run(userSession.UserId);

        base.OnActionExecuting(filterContext);
    }
}