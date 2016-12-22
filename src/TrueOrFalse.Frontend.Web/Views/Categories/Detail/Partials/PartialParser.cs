﻿using System;
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
            var partialJson = GetPartialJson(
                                    match.Value
                                        .Substring(2, match.Value.Length - 4)
                                        .Replace("&quot;", @""""));

            return partialJson == null ? match.Value : GetPartialHtml(partialJson, controllerContext);
        });
    }

    private static PartialJson GetPartialJson(string partialTemplate)
    {
        try
        {
            return JsonConvert.DeserializeObject<PartialJson>(partialTemplate);
        }

        catch
        {
           return null;
        }

        
    }

    public static string GetPartialHtml(PartialJson partialJson, ControllerContext controllerContext)
    {
        if (partialJson.PartialName != "SingleSet") return "";

        var set = Sl.R<SetRepo>().GetById(partialJson.CategoryId);

        var renderPartialParams = new RenderPartialParams {PartialName = "SingleSet", Model = new SingleSetModel(set)};

        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/" + renderPartialParams.PartialName + ".ascx",
            renderPartialParams.Model,
            controllerContext);
    }

}