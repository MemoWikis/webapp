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
        return MarkdownMarkdig.ToHtml(markdown);
        //return "<div class='row'><div class='col-sm-4'>" + PartialParser.Run(markdown, controllerContext) + "</div></div>";
    }
}
