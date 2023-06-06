using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TrueOrFalse.Web
{
    public class JavaScriptActionDescriptor : ActionDescriptor
    {
        private readonly string _actionName;
        private readonly ControllerDescriptor _controllerDescriptor;

        public JavaScriptActionDescriptor(string actionName, ControllerDescriptor controllerDescriptor)
        {
            _actionName = actionName;
            _controllerDescriptor = controllerDescriptor;
        }

        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            return new ViewResult();
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return new ParameterDescriptor[0];
        }

        public override string ActionName
        {
            get { return _actionName; }
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get { return _controllerDescriptor; }
        }
    }
}