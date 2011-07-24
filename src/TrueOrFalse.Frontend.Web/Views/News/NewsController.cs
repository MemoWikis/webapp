using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class NewsController : Controller
{
    public ActionResult News()
    {
        return View(new NewsModel());

    }
}
