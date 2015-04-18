using System.Web.Mvc;

public class ErrorController : Controller
{
    public ActionResult _404()
    {
        return View(new BaseModel());
    }

    public ActionResult _500()
    {
        return View(new BaseModel());
    }
}