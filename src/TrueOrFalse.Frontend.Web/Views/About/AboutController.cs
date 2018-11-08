using System.Web.Mvc;
public class AboutController : Controller
{
    [SetMenu(MenuEntry.About)]
    public ActionResult AboutMemucho()
    {
        return View(new AboutMemuchoModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult ForTeachers()
    {
        return View(new ForTeachersModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult Jobs()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult Master()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult WelfareCompany()
    {
        return View(new BaseModel());
    }


}