using System.Web.Mvc;

public class AccessBetaAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var actionName = filterContext.ActionDescriptor.ActionName;
        var controllerName = filterContext.Controller.GetType().Name;
        var checkAccess =
            controllerName != "AppController" && 
            controllerName != "BetaController" && 
            controllerName != "VariousPublicController" &&
            actionName != "RemoteLogin";

        var userSession = new SessionUser();
        if (checkAccess && !userSession.HasBetaAccess)
            filterContext.Result = new RedirectResult("/beta");
        
        base.OnActionExecuting(filterContext);
    }
}
