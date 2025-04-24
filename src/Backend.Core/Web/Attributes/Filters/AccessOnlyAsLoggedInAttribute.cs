using Microsoft.AspNetCore.Mvc.Filters;
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
        if (!_sessionUser.IsLoggedIn)
            throw new InvalidAccessException();

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(_sessionUser);

        base.OnActionExecuting(filterContext);
    }
}