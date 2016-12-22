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
        result = TemplateParser.Run(result, controllerContext);
        return result;
    }
}
