using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

public class TemplateParser
{
    public static string Run(string stringToParse, ControllerContext controllerContext)
    {
        var regex = new Regex(@"\[\[(.*?)\]\]", RegexOptions.Singleline);//Matches "[[something]]" non-greedily, across multiple lines and only if not nested

        return regex.Replace(stringToParse, match =>
        {
            var templateJson = GetTemplateJson(
                                    match.Value
                                        .Substring(2, match.Value.Length - 4)
                                        .Replace("&quot;", @""""));

            if (templateJson == null)
                return match.Value;

            var html = GetHtml(templateJson, controllerContext);

            return string.IsNullOrEmpty(html) ? match.Value : html;
        });
    }

    private static TemplateJson GetTemplateJson(string template)
    {
        try
        {
            return JsonConvert.DeserializeObject<TemplateJson>(template);
        }

        catch
        {
           return null;
        }
    }

    private static string GetHtml(TemplateJson templateJson, ControllerContext controllerContext)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "singleset":
                return GetPartialHtml(templateJson, controllerContext);
            default:
                return GetElementHtml(templateJson);
        }
    }

    private static string GetPartialHtml(TemplateJson templateJson, ControllerContext controllerContext)
    {
        var partialModel = GetPartialModel(templateJson);

        try
        {
            return ViewRenderer.RenderPartialView(
                "~/Views/Categories/Detail/Partials/" + templateJson.TemplateName + ".ascx",
                partialModel,
                controllerContext);
        }

        catch
        {
            return null;
        }
    }

    private static BaseModel GetPartialModel(TemplateJson templateJson)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "singleset":
                return new SingleSetModel(Sl.R<SetRepo>().GetById(templateJson.SetId));
            default:
                return null;
        }
    }

    private static string GetElementHtml(TemplateJson templateJson)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "divstart":
                return "<div class='" + templateJson.CssClasses + "'>";
            case "divend":
                return "</div>";
            default:
                return null;
        }
    }
}