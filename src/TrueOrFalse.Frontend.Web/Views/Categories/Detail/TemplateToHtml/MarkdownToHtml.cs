using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

public class MarkdownToHtml
{
    public static string Run(string markdown, Category category, ControllerContext controllerContext, int? version = null)
    {
        if(String.IsNullOrEmpty(markdown?.Trim()))
            return "";

        var result = MarkdownContentToHtml(markdown, category, controllerContext, version);
        return result;
    }

    public static string MarkdownContentToHtml(string markdown, Category category, ControllerContext controllerContext, int? version = null)
    {
        var tokens = MarkdownTokenizer.Run(markdown);
        var result = new StringBuilder();
        foreach (Token element in tokens)
        {
            var htmlResult = MarkdownSingleTemplateToHtml.Run(element, category, controllerContext, false, version);

            result.Append(htmlResult);
        }

        return result.ToString();
    }
}