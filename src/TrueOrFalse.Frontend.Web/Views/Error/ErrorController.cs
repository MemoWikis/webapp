using System.Web.Mvc;

public class ErrorController : Controller
{
    public ActionResult _404() => View(new BaseModel());
    public ActionResult _500() => View(new BaseModel());

    public ActionResult _NotLoggedIn() => View(new BaseModel());
}