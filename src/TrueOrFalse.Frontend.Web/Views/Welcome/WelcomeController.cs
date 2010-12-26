using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

namespace TrueOrFalse.Frontend.Web.Controllers
{
    [HandleError]
    public class WelcomeController : Controller
    {
        private readonly IQuestionService _questionService;        

        public WelcomeController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public ActionResult Welcome()
        {
            ViewData["Message"] = "Richtig oder falsch?";

            var model = new QuestionHomeModel();
            model.MostPopular = QuestionDemoData.All();

            return View(model);
        }


    }
}
