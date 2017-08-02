using System;
using System.Web.Mvc;

public class LomApiController : BaseController
{
    public ActionResult GetTopic(int id)
    {
        var category = Sl.CategoryRepo.GetById(id);
        var xml = LomXml.From(category);

        return Content(xml, "text/xml");
    }


    /// <summary>
    /// Local access only,  for debugging and testing.
    /// </summary>
    public ActionResult TriggerExportToFileSystem()
    {
        if(!Request.IsLocal)
            throw new Exception("only local access allowed");

        LomExporter.AllToFileSystem();

        return Content("DONE!");
    }
}
