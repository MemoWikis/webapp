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
        private readonly IQuestionRepository _questionRepository;        

        public WelcomeController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public ActionResult Welcome()
        {
            ViewData["Message"] = "Richtig oder falsch?";

            var model = new QuestionHomeModel {MostPopular = QuestionDemoData.All()};

            return View(model);
        }

    }
}
