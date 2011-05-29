using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueOrFalse.View.Web.Views.Summary
{
    public class SummaryController : Controller
    {
        //
        // GET: /Summary/

        public ActionResult Summary()
        {
            return View(new SummaryModel());
        }

    }
}
