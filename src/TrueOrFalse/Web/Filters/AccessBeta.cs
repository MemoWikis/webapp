using System.Web.Mvc;
using TrueOrFalse.Web.Context;

public class AccessBeta : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var controllerName = filterContext.Controller.GetType().Name;
        var checkAccess = controllerName != "BetaController" && controllerName != "VariousPublicController";

        var userSession = new SessionUser();
        if (checkAccess && !userSession.HasBetaAccess)
            filterContext.Result = new RedirectResult("/beta");
        
        base.OnActionExecuting(filterContext);
    }
}
