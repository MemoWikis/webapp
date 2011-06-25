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
    public static class Buttons
    {
        public static string Submit(string buttonText, bool inline = true)
        {
            return String.Format("<div class='button' {0}><a href='#' class='submit button ui-state-default ui-corner-all'><span class='ui-icon ui-icon-triangle-1-e'></span>{1}</a></div>",
                inline == true ? "style='display:inline;'" : "",
                buttonText);            
        }

        public static string Link(string buttonText, 
                                  string actionName, 
                                  string controllerName, 
                                  bool inline = false)
        {   
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return String.Format("<div class='button' {0}><a href='{1}' class='button ui-state-default ui-corner-all'><span class='ui-icon ui-icon-triangle-1-e'></span>{2}</a></div>",
                inline == true ? "style='display:inline;'" : "",
                urlHelper.Action(actionName, controllerName), 
                buttonText);
        }
    }
}
