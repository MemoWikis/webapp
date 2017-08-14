using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Web;

namespace System.Web.Mvc
{
    public static class MessageHtml
    {
        public static void Message(this HtmlHelper html, UIMessage message)
        {
            if (message == null)
                return;

            var newViewContext =  
                new ViewContext(html.ViewContext, 
                html.ViewContext.View, 
                new ViewDataDictionary(message), 
                html.ViewContext.TempData, 
                html.ViewContext.Writer);


            ViewEngineResult result = ViewEngines.Engines.FindPartialView(
                html.ViewContext, "~/Views/Shared/Message.ascx");
            
            result.View.Render(newViewContext, html.ViewContext.Writer); 

        }
    }
}
