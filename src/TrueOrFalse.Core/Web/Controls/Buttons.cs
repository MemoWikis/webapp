namespace System.Web.Mvc
{
    public static class Buttons
    {
        public static string Submit(string buttonText, string id = null, string url = null, bool inline = true)
        {
            return String.Format("<div class='button' {0}><a href='{2}' {1} class='submit button ui-state-default ui-corner-all'><span class='ui-icon ui-icon-triangle-1-e'></span>{3}</a></div>",
                inline ? "style='display:inline;'" : "",
                id != null ? "id='"+ id +"'" : "",
                url ?? "#",
                buttonText);            
        }

        public static string Link(string buttonText, 
                                  string actionName, 
                                  string controllerName,
                                  ButtonIcon buttonIcon = ButtonIcon.Link,
                                  bool inline = false)
        {   
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return String.Format("<div class='button' {0}><a href='{1}' class='button ui-state-default ui-corner-all'><span class='ui-icon {2}'></span>{3}</a></div>",
                inline == true ? "style='display:inline;'" : "",
                urlHelper.Action(actionName, controllerName), 
                GetIconCss(buttonIcon),
                buttonText);
        }

        private static string GetIconCss(ButtonIcon buttonIcon)
        {
            switch(buttonIcon)
            {
                case ButtonIcon.Add:
                    return "ui-icon-circle-plus";

                case ButtonIcon.Delete:
                    return "ui-icon-circle-minus";

                case ButtonIcon.Link:
                    return "ui-icon-triangle-1-e";

                case ButtonIcon.Settings:
                    return "ui-icon-wrench";

                case ButtonIcon.Edit:
                    return "ui-icon-pencil";
            }

            throw new Exception("unknown button style");
        }    
    }
}
