using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;

public class VariousPublicController : Controller
{
    public ActionResult Imprint()
    {
        return View(new BaseModel());
    }

    public ActionResult NotDoneYet()
    {
        return View(new BaseModel());
    }

    public ActionResult WelfareCompany()
    {
        return View(new BaseModel());
    }

    public ActionResult Qunit()
    {
        return View(new BaseModel());
    }
}

