using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

public class TemplateToHtml
{
    public static string Run(List<TemplateJson> tokens, Category category, ControllerContext controllerContext, int? version = null)
    {
        var result = new StringBuilder();
        foreach (TemplateJson token in tokens)
        {
            var htmlResult = Run(token, category, controllerContext, false, version);

            result.Append(htmlResult);
        }

        return result.ToString();
    }

    public static string Run(Category category, ControllerContext controllerContext, int? version = null)
    {
        return GetInlineTextHtml(category.Content, controllerContext);
    }

    public static string Run(TemplateJson templateJson, Category category, ControllerContext controllerContext, bool preview = false, int? version = null)
    {
        try
        {
            if (templateJson == null && !preview)
                return "";

            var baseContentModule = TemplateParserForSingleTemplate.Run(templateJson, category);

            var html = GetHtml(
                baseContentModule,
                category,
                controllerContext
            );

            if (string.IsNullOrEmpty(html))
                throw new Exception("Es konnte kein Html erzeugt werden.");

            return html;

        }
        catch (Exception e)
        {
            if (version == null)
                Logg.r().Error(e, $"Fehler beim Parsen der Kategorie Id={category.Id}.");

            return GetReplacementForNonparsableTemplate(templateJson.OriginalJson, e.Message);
        }
    }

    public static string GetHtml(BaseContentModule contentModule, Category category, ControllerContext controllerContext)
    {
        var templateJson = contentModule.TemplateJson;

        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
            case "textblock":
            case "inlinetext":
                return GetPartialHtml(contentModule, category, controllerContext);
            default:
            {
                var elementHtml = GetElementHtml(templateJson);

                if (string.IsNullOrEmpty(elementHtml))
                    throw new Exception($"Name des Templates '{templateJson.TemplateName}' ist unbekannt.");

                return elementHtml;
            }
        }
    }


    private static string GetPartialHtml(
        BaseContentModule contentModule,
        Category category,
        ControllerContext controllerContext)
    {
        return GetPartialHtml(contentModule.TemplateJson, controllerContext, contentModule);
    }

    private static string GetPartialHtml(TemplateJson templateJson, ControllerContext controllerContext, BaseModel partialModel)
    {
        return ViewRenderer.RenderPartialView(
            $"~/Views/Categories/Detail/Partials/{templateJson.TemplateName}/{templateJson.TemplateName}.ascx",
            partialModel,
            controllerContext);
    }

    private static string GetInlineTextHtml(string categoryContent, ControllerContext controllerContext)
    {
        var inlineTextModel = new InlineTextModel(categoryContent);
        return ViewRenderer.RenderPartialView(
            $"~/Views/Categories/Detail/Partials/InlineText/InlineText.ascx",
            inlineTextModel,
            controllerContext);
    }



    private static string GetElementHtml(TemplateJson templateJson)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "wishknowledgeinthebox":
                return "<div class = 'wishKnowledgeTemplate'></div>";
            default:
                return null;
        }
    }

    private static string GetReplacementForNonparsableTemplate(string match, string exceptionMessage)
    {
        return Sl.SessionUser.IsInstallationAdmin
            ? $"<div style=\'background-color: rgba(130, 8, 22, 0.33); margin-bottom: 20px;\'>Folgendes Template konnte nicht umgewandelt werden:<div>{match}</div>Fehler: {exceptionMessage}</div>"
            : "";
    }
}