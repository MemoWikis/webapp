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
    public static string Run(string stringToParse, Category category, ControllerContext controllerContext)
    {
        var regex = new Regex(@"\[\[(.*?)\]\]", RegexOptions.Singleline);//Matches "[[something]]" non-greedily across multiple lines and only if not nested

        return regex.Replace(stringToParse, match =>
        {
            var templateJson = GetTemplateJson(
                                    match.Value
                                        .Substring(2, match.Value.Length - 4)
                                        .Replace("&quot;", @""""));

            if (templateJson == null)
                return match.Value;

            var html = GetHtml(templateJson, category, controllerContext);

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

    private static string GetHtml(TemplateJson templateJson, Category category, ControllerContext controllerContext)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "categorynetwork":
            case "contentlists":
            case "singleset":
            case "setlistcard":
                return GetPartialHtml(templateJson, category, controllerContext);
            default:
                return GetElementHtml(templateJson);
        }
    }

    private static string GetPartialHtml(TemplateJson templateJson, Category category, ControllerContext controllerContext)
    {
        var partialModel = GetPartialModel(templateJson, category);

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

    private static BaseModel GetPartialModel(TemplateJson templateJson, Category category)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "categorynetwork":
            case "contentlists":
                return new CategoryModel(category);
            case "singleset":
                return new SingleSetModel(Sl.R<SetRepo>().GetById(templateJson.SetId), setText: templateJson.SetText);
            case "setlistcard":
                return new SetListCardModel(templateJson.SetList, templateJson.Title, templateJson.Description, templateJson.RowCount);
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