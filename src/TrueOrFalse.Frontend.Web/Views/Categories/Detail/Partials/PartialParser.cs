using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

public class PartialParser
{
    public static string Run(string stringToParse, ControllerContext controllerContext)
    {
        var regex = new Regex(@"\[\[(.*?)\]\]", RegexOptions.Singleline);//Matches "[[something]]" non-greedily, across multiple lines and only if not nested

        return regex.Replace(stringToParse, match =>
        {
            var partialString = GetPartial(
                                    match.Value
                                        .Substring(2, match.Value.Length - 4)
                                        .Replace("&quot;", @""""),
                                    controllerContext);

            return string.IsNullOrEmpty(partialString) ? match.Value : partialString;
        });
    }

    private static string GetPartial(string partialTemplate, ControllerContext controllerContext)
    {
        PartialJson partialJson;

        try
        {
            partialJson = JsonConvert.DeserializeObject<PartialJson>(partialTemplate);
        }

        catch
        {
           return "";
        }

        return GetSetCollectionPartial(partialJson, controllerContext);
    }

    public static string GetSetCollectionPartial(PartialJson partialJson, ControllerContext controllerContext)
    {
        if (partialJson.PartialName != "SetCollection") return "";

        var set = Sl.R<SetRepo>().GetById(partialJson.CategoryId);

        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/SetCollection.ascx",
            new SetCollectionModel(set),
            controllerContext);
    }
}