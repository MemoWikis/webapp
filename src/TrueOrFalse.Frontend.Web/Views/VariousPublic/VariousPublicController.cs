using System.Web.Mvc;

public class VariousPublicController : Controller
{
    public ActionResult Imprint()
    {
        return View(new BaseModel());
    }

    [SetMainMenu(MainMenuEntry.None)]
    public ActionResult MemuchoBeta()
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

    public ActionResult Qunit()
    {
        return View(new BaseModel());
    }
}

