using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;

namespace TrueOrFalse.View.Web.Views
{
    public class ImprintController : Controller
    {
        public ActionResult Imprint()
        {
            return View(new ModelBase());
        }
    }
}
