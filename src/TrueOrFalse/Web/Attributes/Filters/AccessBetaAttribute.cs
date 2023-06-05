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
            controllerName != "ForwardController" &&
            actionName != "RemoteLogin";

        if (checkAccess && !SessionUserLegacy.HasBetaAccess)
            filterContext.Result = new RedirectResult("/beta");
        
        base.OnActionExecuting(filterContext);
    }
}