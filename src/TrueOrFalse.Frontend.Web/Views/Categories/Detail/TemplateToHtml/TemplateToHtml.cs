using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

public class TemplateToHtml
{
    public static string Run(Category category, ControllerContext controllerContext)
    {
        var html = GetInlineTextHtml(category, controllerContext);
        if (string.IsNullOrEmpty(html))
            throw new Exception("Es konnte kein Html erzeugt werden.");
        return html;
    }
    
    private static string GetInlineTextHtml(Category category, ControllerContext controllerContext)
    {
        var inlineTextModel = new InlineTextModel(category);
        return ViewRenderer.RenderPartialView(
            $"~/Views/Categories/Detail/Partials/InlineText/InlineText.ascx",
            inlineTextModel,
            controllerContext);
    }
}