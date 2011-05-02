using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class Validation : Controller
    {
        //
        // GET: /Validation/

        public ActionResult IsUserNameAvailable()
        {
            return View();
        }

    }
}
