using System;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class MarkdownToHtml
{
    public static string Run(string markdown, Category category, ControllerContext controllerContext)
    {
        if(String.IsNullOrEmpty(markdown?.Trim()))
            return "";

        var result = MarkdownMarkdig.ToHtml(markdown);
        result = TemplateParser.Run(result, category, controllerContext);
        return result;
    }
}
