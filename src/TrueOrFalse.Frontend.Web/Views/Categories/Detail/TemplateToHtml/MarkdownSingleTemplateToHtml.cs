﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

public class MarkdownSingleTemplateToHtml
{
    public static string Run(Token token, Category category, ControllerContext controllerContext)
    {
        return Run(token.ToText(), category, controllerContext);
    }

    public static string Run(string stringToParse, Category category, ControllerContext controllerContext)
    {
        try
        {
            var baseContentModule = TemplateParserForSingleTemplate.Run(stringToParse, category);

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
            Logg.r().Error($"Fehler beim Parsen der Kategorie Id={category.Id} ({e.Message} {e.StackTrace}).");
            return GetReplacementForNonparsableTemplate(stringToParse, e.Message);
        }
    }

    private static string GetHtml(BaseContentModule contentModule, Category category, ControllerContext controllerContext)
    {
        var templateJson = contentModule.TemplateJson;

        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
            case "medialist":
            case "videowidget":
            case "settestsessionnostartscreen":
            case "singlesetfullwidth":
            case "singlecategoryfullwidth":
            case "categorynetwork":
            case "contentlists":
            case "relatedcontentlists":
            case "educationofferlist":
            case "setlistcard":
            case "setcardminilist":
            case "singlecategory":
            case "singlequestionsquiz":
            case "spacer":
            case "textblock":
            case "cards":
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

    /// <summary>
    /// /If template cannot be parsed show it for admins, otherwise hide it
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    private static string GetReplacementForNonparsableTemplate(string match, string exceptionMessage)
    {
        return Sl.SessionUser.IsInstallationAdmin 
            ? $"<div style=\'background-color: rgba(130, 8, 22, 0.33); margin-bottom: 20px;\'>Folgendes Template konnte nicht umgewandelt werden:<div>{match}</div>Fehler: {exceptionMessage}</div>"
            : "";
    }
}