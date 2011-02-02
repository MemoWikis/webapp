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
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public ActionResult CreateQuestion()
        {
            var model = new CreateQuestionModel();
            model.Answer = "Antwort eingeben";
            model.Question = "Frage eingeben";

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateQuestion(CreateQuestionModel model)
        {
            ViewData["question"] = model.Question;

            _questionRepository.Create(model.ConvertToQuestion());

            return View(model);
        }

    }
}
