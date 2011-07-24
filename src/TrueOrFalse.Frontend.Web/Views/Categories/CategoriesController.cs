using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class CategoriesController : Controller
{
    public ActionResult Categories()
    {
        return View(new CategoriesModel());

    }
}
