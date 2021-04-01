using System;
using System.Web.Mvc;

public class TemplateToHtml
{
    public static string Run(CategoryCacheItem categoryCacheItem, ControllerContext controllerContext)
    {
        var html = GetInlineTextHtml(categoryCacheItem, controllerContext);
        if (string.IsNullOrEmpty(html))
            throw new Exception("Es konnte kein Html erzeugt werden.");
        return html;
    }
    
    private static string GetInlineTextHtml(CategoryCacheItem category, ControllerContext controllerContext)
    {
        var inlineTextModel = new InlineTextModel(category);
        return ViewRenderer.RenderPartialView(
            $"~/Views/Categories/Detail/Partials/InlineText/InlineText.ascx",
            inlineTextModel,
            controllerContext);
    }
}