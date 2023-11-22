
using Microsoft.AspNetCore.Mvc.Filters;

public class AccessOnlyAsAdminAttribute : Attribute{}

public class AccessOnlyAsAdminAttributeFilter : ActionFilterAttribute
{
    private readonly SessionUser _sessionUser;

    public AccessOnlyAsAdminAttributeFilter(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!_sessionUser.IsInstallationAdmin)
            throw new InvalidAccessException();

        base.OnActionExecuting(filterContext);
    }
}