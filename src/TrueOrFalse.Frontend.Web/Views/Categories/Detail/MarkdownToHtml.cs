using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class MarkdownToHtml
{
    public static string Run(string markdown, ControllerContext controllerContext)
    {
        var result = MarkdownMarkdig.ToHtml(markdown);
        result = "<div class='row'><div class='col-sm-4'>" + TemplateParser.Run(result, controllerContext) + "</div></div>";
        return result;
    }
}
