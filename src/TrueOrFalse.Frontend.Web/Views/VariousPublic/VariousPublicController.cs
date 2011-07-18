using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;

public class VariousPublicController : Controller
{
    public ActionResult Imprint()
    {
        return View(new ModelBase().ShowLeftMenu_TopUsers().ShowRightLogin());
    }

    public ActionResult NotDoneYet()
    {
        return View(new ModelBase().ShowLeftMenu_Empty());
    }

    public ActionResult WelfareCompany()
    {
        return View(new ModelBase().ShowLeftMenu_Empty().ShowRightLogin());
    }
}

