using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;

namespace TrueOrFalse.Frontend.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        private SampleData _sampleData;

        public HomeController(SampleData sampleData) 
        {
            _sampleData = sampleData;
        }

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
                _sampleData.CreateLogins();
                ViewBag.Message = "Beispieldaten wurden angelegt";       
            }

            return View();
        }
    }
}
