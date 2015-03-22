using System.Web.Mvc;

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

    public ActionResult Beta()
    {
        return View();
    }
}

