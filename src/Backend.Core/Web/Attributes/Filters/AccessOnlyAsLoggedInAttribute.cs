using Microsoft.AspNetCore.Mvc.Filters;

public class AccessOnlyAsLoggedInAttribute : Attribute;

public class AccessOnlyAsLoggedInFilter(SessionUser _sessionUser)
    : ActionFilterAttribute, IRegisterAsInstancePerLifetime
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!_sessionUser.IsLoggedIn)
            throw new InvalidAccessException();

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(_sessionUser);

        base.OnActionExecuting(filterContext);
    }
}