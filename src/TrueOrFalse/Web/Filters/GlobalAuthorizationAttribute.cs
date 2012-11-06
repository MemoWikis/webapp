using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Web.Context;

namespace System.Web.Mvc
{
    public class GlobalAuthorizationAttribute : ActionFilterAttribute
    {
        readonly List<string> _publicControllers = new List<string> { "Welcome", "Imprint", "Persona", "VariousPublic", "Export" }; 

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_publicControllers.Any(item => item == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName))
                return;

            var userSession = new SessionUser();
            if (!userSession.IsLoggedIn)
                filterContext.HttpContext.Response.Redirect("/Welcome/Login", true);

            base.OnActionExecuting(filterContext);
        }
    }
}
