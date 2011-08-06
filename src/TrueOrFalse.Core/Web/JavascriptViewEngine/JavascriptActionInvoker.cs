using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TrueOrFalse.Core.Web
{
    public class JavaScriptActionInvoker : ControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            var action = base.FindAction(controllerContext, controllerDescriptor, actionName);
            if (action != null)
            {
                return action;
            }

            if (actionName.EndsWith(".js"))
            {
                return new JavaScriptActionDescriptor(actionName, controllerDescriptor);
            }

            else
                return null;
        }
    }
}
