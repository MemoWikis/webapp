using System.Web.Mvc;
public class AccessOnlyAsLoggedInAttribute : Attribute { }
public class AccessOnlyAsLoggedInFilter : ActionFilterAttribute,IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;

    public AccessOnlyAsLoggedInFilter(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!SessionUserLegacy.IsLoggedIn)
            throw new InvalidAccessException();

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(_sessionUser);

        base.OnActionExecuting(filterContext);
    }
}