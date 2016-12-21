using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

public class PartialParser
{
    public static string Run(string stringToParse)
    {
        var regex = new Regex(@"\[\[(.*?)\]\]");//Matches "[[something]]" non-greedily and only if not nested

        return regex.Replace(stringToParse, match =>
        {
            var partialString = GetPartial(match.Value.Substring(2,match.Value.Length - 4));

            return string.IsNullOrEmpty(partialString) ? match.Value : partialString;
        });
    }

    public static string Run(string stringToParse, ControllerContext controllerContext)
    {
        return Run(stringToParse);
    }

    public static MatchCollection ExtractPartialTemplates(string text)
    {
        var regex = new Regex(@"\[\[(.*?)\]\]");

        return regex.Matches(text);
    }

    private static string GetPartial(string partialTemplate)
    {
        return partialTemplate.ToUpper();
    }

    public static string ReturnSetCollectionPartial(string stringToParse, ControllerContext controllerContext)
    {
        var set = Sl.R<SetRepo>().GetById(9);

        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/SetCollection.ascx",
            new SetCollectionModel(set),
            controllerContext);
    }
}