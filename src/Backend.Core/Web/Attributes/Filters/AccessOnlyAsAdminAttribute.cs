using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[ServiceFilter(typeof(AccessOnlyAsAdminFilter))]
public class AccessOnlyAsAdminAttribute : Attribute{ }

public class AccessOnlyAsAdminFilter(SessionUser _sessionUser) : ActionFilterAttribute,IRegisterAsInstancePerLifetime
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!_sessionUser.IsInstallationAdmin)
            throw new InvalidAccessException();

        base.OnActionExecuting(filterContext);
    }
}