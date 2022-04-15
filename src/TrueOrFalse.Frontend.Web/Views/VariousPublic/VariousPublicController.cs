using System.Web.Mvc;

public class VariousPublicController : Controller
{
    public ActionResult Imprint()
    {
        return View(new BaseModel());
    }

    public ActionResult TermsAndConditions()
    {
        return View(new BaseModel());
    }

    public ActionResult NotDoneYet()
    {
        return View(new BaseModel());
    }
}

