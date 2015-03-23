using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class DatesController : BaseController
{
    [SetMenu(MenuEntry.Dates)]
    public ActionResult Dates()
    {
        return View(new DatesModel());
    }
}