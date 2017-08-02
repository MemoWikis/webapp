using System.Web.Mvc;

public class LomApiController : BaseController
{
    public ActionResult GetTopic(int id)
    {
        var category = Sl.CategoryRepo.GetById(id);
        var xml = LomXml.From(category);

        return Content(xml, "text/xml");
    }   
}
