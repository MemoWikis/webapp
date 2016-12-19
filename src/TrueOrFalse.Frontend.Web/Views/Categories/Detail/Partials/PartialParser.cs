using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class PartialParser
{
    public static string Run(string htmlFromMarkdown, ControllerContext controllerContext)
    {
        var set = Sl.R<SetRepo>().GetById(9);

        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/SetCollection.ascx",
            new SetCollectionModel(set),
            controllerContext);
    }
}