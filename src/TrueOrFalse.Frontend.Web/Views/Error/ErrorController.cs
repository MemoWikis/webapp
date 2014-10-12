using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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