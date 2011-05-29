using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

namespace TrueOrFalse.View.Web.Views.Question
{
    [HandleError]
    public class CreateQuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private const string _viewLocation = "~/Views/Question/CreateQuestion/CreateQuestion.aspx";

        public CreateQuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        
        public ActionResult CreateQuestion()
        {
            var model = new CreateQuestionModel();
            model.Answer = "Antwort eingeben";
            model.Question = "Frage eingeben";

            return View(_viewLocation, model);
        }

        [HttpPost]
        public ActionResult CreateQuestion(CreateQuestionModel model)
        {
            ViewData["question"] = model.Question;

            _questionRepository.Create(model.ConvertToQuestion());

            return View(_viewLocation, model);
        }

    }
}