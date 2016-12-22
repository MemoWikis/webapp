using System;
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
            var partialJson = GetTemplateJson(
                                    match.Value
                                        .Substring(2, match.Value.Length - 4)
                                        .Replace("&quot;", @""""));

            return partialJson == null ? match.Value : GetPartialHtml(partialJson, controllerContext);
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

    public static string GetPartialHtml(TemplateJson templateJson, ControllerContext controllerContext)
    {
        if (templateJson.PartialName != "SingleSet") return "";

        var set = Sl.R<SetRepo>().GetById(templateJson.CategoryId);

        var renderPartialParams = new RenderPartialParams {PartialName = "SingleSet", Model = new SingleSetModel(set)};

        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/" + renderPartialParams.PartialName + ".ascx",
            renderPartialParams.Model,
            controllerContext);
    }

}