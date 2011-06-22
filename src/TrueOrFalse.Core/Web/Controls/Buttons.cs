using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web.Routing;
using Seedworks.Web.State;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{

    public class BlaModel
    {
        
    }

    public class Buttons
    {
        public static string Link(string buttonText, string actionName, string controllerName)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            
            return String.Format("<div class='button'><a href='{0}' class='button ui-state-default ui-corner-all'><span class='ui-icon ui-icon-triangle-1-e'></span>{1}</a></div>",
                urlHelper.Action(actionName, controllerName), 
                buttonText);
        }
    }
}
