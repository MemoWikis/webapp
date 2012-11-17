using TrueOrFalse.Web.Context;

namespace System.Web.Mvc
{
    public class AccessOnlyAsAdminAndLocalAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!HttpContext.Current.Request.IsLocal)
                throw new Exception("only local access is allowed");

            var userSession = new SessionUser();
            if (!userSession.User.IsInstallationAdmin)
                throw new Exception("only local access is allowed");

            base.OnActionExecuting(filterContext);
        }
    }
}
