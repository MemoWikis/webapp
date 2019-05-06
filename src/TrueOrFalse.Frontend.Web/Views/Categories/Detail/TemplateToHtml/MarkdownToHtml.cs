using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

public class MarkdownToHtml
{
    public static string Run(string markdown, Category category, ControllerContext controllerContext)
    {
        if(String.IsNullOrEmpty(markdown?.Trim()))
            return "";

        var result = MarkdownContentToHtml(markdown, category, controllerContext);
        return result;
    }

    public static string MarkdownContentToHtml(string markdown, Category category, ControllerContext controllerContext)
    {
        var parts = MarkdownTokenizer.SplitMarkdown(markdown);
        var result = new StringBuilder();
        foreach (Part element in parts)
        {
            var htmlResult = MarkdownSingleTemplateToHtml.Run(element, category, controllerContext);

            result.Append(htmlResult);
        }

        return result.ToString();
    }
}