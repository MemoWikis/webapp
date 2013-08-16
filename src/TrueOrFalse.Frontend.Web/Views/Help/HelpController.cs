using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class HelpController : Controller
{
    public ActionResult Willkommen(){ return View(new HelpModel()); }
    public ActionResult DatenSicherheit(){ return View(new HelpModel()); }
    public ActionResult Reputation() { return View(new HelpModel()); }
}
