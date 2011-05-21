using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueOrFalse.Frontend.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string createSampleData)
        {
            if (createSampleData != null)
            {
                ViewBag.Message = "Beispieldaten wurden angelegt";       
            }

            return View();
        }
    }
}
