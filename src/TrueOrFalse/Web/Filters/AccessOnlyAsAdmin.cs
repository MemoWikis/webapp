using TrueOrFalse.Web.Context;

namespace System.Web.Mvc
{
    public class AccessOnlyAsAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUser();
            if (!userSession.IsInstallationAdmin)
                throw new Exception("only local access is allowed");

            base.OnActionExecuting(filterContext);
        }
    }
}
