using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Tests.Answer;

namespace TrueOrFalse.Frontend.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IQuestionService _questionService;

        public HomeController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult CreateQuestion()
        {
            var model = new QuestionCreateModel();
            model.Answer = "Antwort eingeben";
            model.Question = "Frage eingeben";

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateQuestion(QuestionCreateModel model)
        {
            ViewData["question"] = model.Question;

            _questionService.Create(model.ConvertToQuestion());

            return View(model);
        }

        public ActionResult UpdateQuestion()
        {
            return View();
        }
    }
}
