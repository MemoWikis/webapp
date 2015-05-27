using System.Web.Mvc;

namespace TrueOrFalse.Web
{
    public class JavaScriptActionInvoker : ControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(
            ControllerContext controllerContext, 
            ControllerDescriptor controllerDescriptor, 
            string actionName)
        {
            var action = base.FindAction(controllerContext, controllerDescriptor, actionName);
            if (action != null)
                return action;

            if (actionName.EndsWith(".js"))
                return new JavaScriptActionDescriptor(actionName, controllerDescriptor);

            return null;
        }
    }
}
